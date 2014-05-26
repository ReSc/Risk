using System;
using System.Collections.Generic;
using System.Linq;
using Risk.Core;
using Risk.Models;

namespace Risk.Players.RS
{
    /// <summary>
    ///     This RiskAnalyzer plays a dangerous game.
    ///     It's focussed on getting and keeping whole continents, so it can get the troop bonus
    ///     It ignores strong countries, instead going for weak countries to attack.
    ///     And it's not very clever about defence too.
    ///     Also, it does nothing with game and turn history.
    ///     I, the programmer, always lost at Risk, so I'm afraid this code reflects that ;-) ...
    ///     The different analysis steps are a potential candidate for extraction into their own classes.
    /// </summary>
    public class RiskAnalyzer
    {
        private readonly IPlayer _player;

        public RiskAnalyzer(IPlayer player)
        {
            _player = player;
        }

        /// <summary>
        ///     Performs analysis of the game state.
        ///     It does a full analysis for every phase of a turn.
        ///     This wastes some computational resources, but hopefully leads to a better outcome.
        /// </summary>
        public RiskAnalysis Analyze(TurnManager turnManager)
        {
            var analysis = new RiskAnalysis();
            // the ordering of the analysis steps is important
            // each step depends on the results of the previous steps.

            // first, check the state of the board.
            DoContinentAnalysis(turnManager, analysis);
            // next, find which countries we want.
            DoAttackAnalysis(turnManager, analysis);
            // then check if we need to move troops
            DoMoveAnalysis(turnManager, analysis);
            // then place reinforcements on the board.
            DoDeployAnalysis(turnManager, analysis);

            // gather important countries.
            DoImportantCountryAnalysis(turnManager, analysis);


            return analysis;
        }

        private void DoContinentAnalysis(TurnManager turnManager, RiskAnalysis analysis)
        {
            // compute continent statistics
            var continents = from c in turnManager.GetGameInfo().GetAllCountries()
                             group c by c.Continent
                                 into continent
                                 select new Continent
                                     {
                                         Name = continent.Key,
                                         MyCountries =
                                             continent.Where(x => x.Owner == _player).ToList(),
                                         EnemyCountries =
                                             continent.Where(x => x.Owner != _player)
                                                      .OrderBy(x => x.NumberOfTroops)
                                                      .ToList(),
                                         Opponents =
                                             continent.Where(x => x.Owner != _player)
                                                      .Distinct(x => x.Owner)
                                                      .ToList()
                                     };

            analysis.Continents.AddRange(continents);
        }

        private void DoAttackAnalysis(TurnManager turnManager, RiskAnalysis analysis)
        {
            var continents = (from c in analysis.GetContinentsByDominance()
                              select c).ToList();

            if (continents.Count == 0) return; // shouldn't happen unless we've won, or have been defeated completely.

            var attacked = new HashSet<Country>();
            var gameInfo = turnManager.GetGameInfo();
            foreach (var continent in continents)
            {
                // create a lookup with the ranking of the enemies,
                // 0 is weakest...
                var enemies = continent.EnemyCountries.OrderBy(x => x.NumberOfTroops)
                                       .Select((country, rank) => new { country, rank })
                                       .ToDictionary(key => key.country, val => val.rank);

                // order friendlies by strongest first
                var friendlies = continent.MyCountries.OrderByDescending(x => x.NumberOfTroops).ToList();

                foreach (var friendly in friendlies)
                {
                    var neighbours = gameInfo.GetAdjacentCountriesWithDifferentOwner(friendly).OrderBy(x =>
                        {
                            // use TryGetValue, adjacent countries can be on a different continent.
                            // use a very high ranking for those, so we don't attack them, yet.
                            var rank = 0;
                            return enemies.TryGetValue(x, out rank) ? rank : 100;
                        }).ToList();


                    if (neighbours.Count == 0)
                        continue; // because we're surrounded by friendlies...

                    // only attack countries not already under attack.
                    var enemy = neighbours.FirstOrDefault(x => !attacked.Contains(x));
                    if (enemy != null)
                    {
                        attacked.Add(enemy);
                        AddAttack(turnManager, analysis, friendly, enemy);
                    }
                }
            }

            if (attacked.Count == 0)
            {
                // break out of owned continents
                var fromGateways = from c in gameInfo.GetAllCountriesOwnedByPlayer(_player)
                                   where c.IsContinentGatewayUnderAttack()
                                   select c;
                foreach (var friendly in fromGateways)
                {
                    var enemy = gameInfo.GetAdjacentCountriesWithDifferentOwner(friendly)
                                               .OrderBy(x => friendly.NumberOfTroops - x.NumberOfTroops)
                                               .FirstOrDefault();
                    if (enemy != null && attacked.Add(enemy))
                        AddAttack(turnManager, analysis, friendly, enemy);
                }
            }


            analysis.Attacks
                    .Sort((a, b) =>
                          (b.FromCountry.NumberOfTroops - b.ToCountry.NumberOfTroops) -
                          (a.FromCountry.NumberOfTroops - a.ToCountry.NumberOfTroops));
        }

        /// <summary>
        ///     Adds attack info to the analysis, but only if there is a chance for success
        /// </summary>
        private void AddAttack(TurnManager turnManager, RiskAnalysis analysis, Country friendly, Country enemy)
        {
            var minDefenders = 2;
            if (friendly.NumberOfTroops > minDefenders+1 && friendly.NumberOfTroops > enemy.NumberOfTroops )
            {
                analysis.Attacks.Add(new Attack(turnManager)
                    {
                        FromCountry = friendly,
                        ToCountry = enemy,
                        Troops = friendly.NumberOfTroops - minDefenders,
                    });
            }
        }

        private void DoDeployAnalysis(TurnManager turnManager, RiskAnalysis analysis)
        {
            var gameInfo = turnManager.GetGameInfo();
            var troops = gameInfo.GetNumberOfUnitsToDeploy(_player);
            troops = troops - AustraliaRuleZ(turnManager, analysis, troops);
            troops = troops - DeployContinentGatewayDefence(turnManager, analysis, troops);
            troops = troops - DeployOffence(turnManager, analysis, troops);
            troops = troops - RescueLastManStanding(turnManager, analysis, troops);
            DeployRemaining(turnManager, analysis, troops, gameInfo);
        }

        private int RescueLastManStanding(TurnManager turnManager, RiskAnalysis analysis, int availableTroops)
        {
            if (availableTroops < 4) return 0;

            var lastManStanding = new Queue<Deployment>(from cnt in analysis.Continents
                                                        where cnt.MyCountries.Sum(x => x.NumberOfTroops) < 3
                                                        from c in cnt.MyCountries
                                                        select
                                                            new Deployment(turnManager)
                                                                {
                                                                    ToCountry = c
                                                                });
            if (lastManStanding.Count == 0)
                return 0;

            while (availableTroops > 0)
            {
                availableTroops--;
                var x = lastManStanding.Dequeue();
                x.Troops++;
                lastManStanding.Enqueue(x);
            }
            analysis.Deployments.AddRange(lastManStanding);
            return lastManStanding.Sum(x => x.Troops);
        }

        private void DeployRemaining(TurnManager turnManager, RiskAnalysis analysis, int troops, GameInformation gameInfo)
        {
            if (troops <= 0) return;
            // deploy remainder evenly...
            var deployments = gameInfo.GetAllCountriesOwnedByPlayer(_player)
                                      .OrderBy(x => x.NumberOfTroops)
                                      .Select(x => new Deployment(turnManager) { ToCountry = x })
                                      .ToList();
            while (troops > 0)
            {
                foreach (var deployment in deployments.TakeWhile(deployment => --troops >= 0))
                {
                    deployment.Troops++;
                }
            }

            analysis.Deployments.AddRange(deployments.Where(x => x.Troops > 0));
        }

        private int AustraliaRuleZ(TurnManager turnManager, RiskAnalysis analysis, int availableTroops)
        {
            if (availableTroops == 0) return 0;
            var downUnder = analysis.Continents.FirstOrDefault(x => x.Name == EContinent.Australia);
            if (downUnder == null || downUnder.MyCountries.Count == 0)
                return 0;

            if (downUnder.IsOwned)
            {
                availableTroops = Math.Min(availableTroops, 3);
                var whatchamacallit = downUnder.MyCountries.First(x => x.IsContinentGateway());
                var minAussieDefenders = 6;
                if (whatchamacallit.IsUnderAttack() || whatchamacallit.NumberOfTroops < minAussieDefenders)
                {
                    analysis.Deployments.Add(new Deployment(turnManager)
                        {
                            ToCountry = whatchamacallit,
                            Troops = availableTroops,

                        });
                    return availableTroops;
                }
                var thingamajig = whatchamacallit.AdjacentCountries.First(x => x.Continent != EContinent.Australia);
                if (thingamajig.IsUnderAttack() || thingamajig.NumberOfTroops < minAussieDefenders)
                {
                    analysis.Deployments.Add(new Deployment(turnManager)
                    {
                        ToCountry = thingamajig,
                        Troops = availableTroops,

                    });
                    return availableTroops;
                }

                return 0;
            }

            var deployments = downUnder.MyCountries.Select(x => new Deployment(turnManager) { ToCountry = x });
            // round-robin deployment, using a queue...
            var q = new Queue<Deployment>(deployments);
            while (availableTroops > 0)
            {
                availableTroops--;
                var c = q.Dequeue();
                c.Troops++;
                q.Enqueue(c);
            }
            analysis.Deployments.AddRange(q);
            return q.Sum(x => x.Troops);
        }

        /// <returns>The number of deployed troops</returns>
        private int DeployContinentGatewayDefence(TurnManager turnManager, RiskAnalysis analysis, int availableTroops)
        {
            if (availableTroops == 0) return 0;
            var gameInformation = turnManager.GetGameInfo();
            var weakGateways = (from c in analysis.Continents
                                where c.IsOwned
                                from country in c.MyCountries
                                where country.IsContinentGateway()
                                // make it a reasonable large margin.
                                let troopTarget = GetTroopTarget(gameInformation, country)
                                where country.NumberOfTroops < troopTarget
                                select new
                                    {
                                        country,
                                        troopTarget,
                                        deployment = new Deployment(turnManager) { ToCountry = country },
                                    }).ToList();

            var deployments = weakGateways.Select(x => x.deployment).ToList();
            var i = 0;

            // deploy troops until there are no more available, or we've reached our target numer of troops everywhere.
            while (deployments.Sum(x => x.Troops) < availableTroops && weakGateways.Count > 0)
            {
                var wg = weakGateways[i];
                wg.deployment.Troops++;
                if (wg.country.NumberOfTroops + wg.deployment.Troops >= wg.troopTarget)
                {
                    weakGateways.RemoveAt(i);
                    if (weakGateways.Count == 0)
                        break;
                }
                i = (i + 1) % weakGateways.Count;
            }

            analysis.Deployments.AddRange(deployments.Where(x => x.Troops > 0));

            return deployments.Sum(x => x.Troops);
        }

        private static int GetTroopTarget(GameInformation gameInformation, Country country)
        {
            var enemies = gameInformation.GetAdjacentCountriesWithDifferentOwner(country).ToList();
            if (enemies.Count == 0) return country.IsContinentGateway() ? 4 : 2;
            if (enemies.Count == 1) return enemies[0].NumberOfTroops + 3;
            return enemies.Sum(x => x.NumberOfTroops) + 1;
        }

        private int DeployOffence(TurnManager turnManager, RiskAnalysis analysis, int availableTroops)
        {
            if (availableTroops == 0) return 0;

            var deployments =
                (// find continent where we're strongest, but not owned
                 from continent in analysis.Continents.Where(x => !x.IsOwned).OrderByDescending(x => x.Dominance)
                 // reinforce weakest countries first.
                 from country in continent.MyCountries.OrderByDescending(x => x.GetThreatLevel())
                 where country.IsUnderAttack()
                 select new Deployment(turnManager) { ToCountry = country }).Take(2).ToList();

            if (availableTroops == 0 || deployments.Count == 0)
                return 0;


            while (deployments.Sum(x => x.Troops) < availableTroops)
            {
                foreach (var deployment in deployments)
                {
                    if (deployments.Sum(x => x.Troops) >= availableTroops)
                        break;

                    deployment.Troops += 1;
                }
            }
            analysis.Deployments.AddRange(deployments);
            return deployments.Sum(x => x.Troops);
        }

        private void DoMoveAnalysis(TurnManager turnManager, RiskAnalysis analysis)
        {
            var gameinfo = turnManager.GetGameInfo();

            var minDefenders = 3;
            var sources = gameinfo.GetAllCountriesOwnedByPlayer(_player)
                        .Where(x => x.NumberOfTroops > minDefenders && !x.IsUnderAttack() && !x.IsContinentGateway())
                        .ToList();
            var movable = 7;
            var targets = new HashSet<Country>();
            foreach (var source in sources)
            {
                var target =
                    gameinfo.GetAdjacentCountriesWithSameOwner(source)
                            .Where(x => x.IsUnderAttack())
                            .OrderBy(x => x.GetThreatLevel())
                            .FirstOrDefault();
                var toMove = Math.Min(movable, (source.NumberOfTroops - minDefenders));
                if (toMove > 0)
                {
                    if (target == null)
                    {
                        var source1 = source;
                        target = gameinfo.GetAdjacentCountriesWithSameOwner(source)
                                         .Where(x => x.IsContinentGateway()
                                                     || source1.NumberOfTroops - x.NumberOfTroops > minDefenders)
                                         .OrderBy(x => x.NumberOfTroops)
                                         .FirstOrDefault(x => targets.Add(x));
                        if (target != null)
                        {
                            toMove = Math.Min(toMove, minDefenders);
                        }
                    }
                    if (target != null)
                    {
                        movable = Math.Max(0, movable - toMove);
                        analysis.Moves.Add(new Move(turnManager)
                        {
                            FromCountry = source,
                            ToCountry = target,
                            Troops = toMove
                        });
                    }
                }
            }
        }

        private void DoImportantCountryAnalysis(TurnManager turnManager, RiskAnalysis analysis)
        {
            var countries = analysis.Attacks.Select(x => x.FromCountry)
                           .Concat(analysis.Moves.Select(x => x.FromCountry))
                           .Concat(analysis.Deployments.Select(x => x.ToCountry))
                           .Concat(
                               turnManager.GetGameInfo().GetAllCountriesOwnedByPlayer(_player).Where(x => x.IsContinentGateway()));

            analysis.ImportantCountries.AddRange(countries);
        }
    }
}