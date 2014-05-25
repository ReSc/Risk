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
            var gameInformation = turnManager.GetGameInfo();
            // the ordering of the analysis steps is important
            // each step depends on the results of the previous steps.

            // first, check the state of the board.
            DoContinentAnalysis(gameInformation, analysis);
            // next, find which countries we want.
            DoAttackAnalysis(turnManager, gameInformation, analysis);
            // then check if we need to move troops
            DoMoveAnalysis(gameInformation, analysis);
            // then place reinforcements on the board.
            DoDeployAnalysis(turnManager, gameInformation, analysis);

            // gather important countries.
            analysis.ImportantCountries
                    .AddRange(FindImportantCountries(gameInformation, analysis));

            return analysis;
        }

        private void DoContinentAnalysis(GameInformation info, RiskAnalysis analysis)
        {
            // compute continent statistics
            var continents = from c in info.GetAllCountries()
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

        private void DoAttackAnalysis(TurnManager turnManager, GameInformation gameInformation, RiskAnalysis analysis)
        {
            var continents = (from c in analysis.GetContinentsByDominance()
                              where c.MyCountries.Count > 0
                              select c).ToList();

            if (continents.Count == 0) return; // shouldn't happen unless we've won, or have been defeated completely.

            var attacked = new HashSet<Country>();
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
                    var neighbours = gameInformation.GetAdjacentCountriesWithDifferentOwner(friendly).OrderBy(x =>
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
                var fromGateways = from c in gameInformation.GetAllCountriesOwnedByPlayer(_player)
                                   where c.IsContinentGatewayUnderAttack()
                                   select c;
                foreach (var friendly in fromGateways)
                {
                    var enemy = gameInformation.GetAdjacentCountriesWithDifferentOwner(friendly)
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
            // kinda simplistic...
            if (friendly.NumberOfTroops > 3 && friendly.NumberOfTroops >= enemy.NumberOfTroops)
            {
                analysis.Attacks.Add(new Attack(turnManager)
                    {
                        FromCountry = friendly,
                        ToCountry = enemy,
                        Troops = Math.Min(3, friendly.NumberOfTroops),
                    });
            }
        }

        private void DoDeployAnalysis(TurnManager turnManager, GameInformation gameInformation, RiskAnalysis analysis)
        {
            var troops = gameInformation.GetNumberOfUnitsToDeploy(_player);
            troops = troops - DeployContinentGatewayDefence(turnManager, gameInformation, analysis, troops);
            troops = troops - DeployOffence(turnManager, gameInformation, analysis, troops);

            if (troops > 0)
            {
                // deploy remainder evenly...
                var deployments = gameInformation.GetAllCountriesOwnedByPlayer(_player)
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
        }

        /// <returns>The number of deployed troops</returns>
        private int DeployContinentGatewayDefence(TurnManager turnManager, GameInformation gameInformation, RiskAnalysis analysis, int availableTroops)
        {
            var turnmanager = turnManager;
            var weakGateways = (from c in analysis.Continents
                                where c.IsOwned
                                from country in c.MyCountries
                                where country.IsContinentGatewayUnderAttack()
                                // make it a reasonable large margin.
                                let troopTarget =
                                    gameInformation.GetAdjacentCountriesWithDifferentOwner(country)
                                                   .Sum(x => x.NumberOfTroops) + 3
                                where country.NumberOfTroops < troopTarget
                                select new
                                    {
                                        country,
                                        troopTarget,
                                        deployment = new Deployment(turnmanager) { ToCountry = country },
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

        private int DeployOffence(TurnManager turnManager, GameInformation gameInformation, RiskAnalysis analysis, int availableTroops)
        {
            var deployments = (from continent in analysis.Continents.Where(x => !x.IsOwned)
                               from country in continent.MyCountries
                               where country.IsUnderAttack()
                               orderby country.GetThreatLevel()
                               select new Deployment(turnManager) { ToCountry = country }).ToList();

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

        private void DoMoveAnalysis(GameInformation gameInformation, RiskAnalysis analysis)
        {
        }

        private IEnumerable<Country> FindImportantCountries(GameInformation gameInformation, RiskAnalysis analysis)
        {
            return analysis.Attacks.Select(x => x.FromCountry)
                           .Concat(analysis.Moves.Select(x => x.FromCountry))
                           .Concat(analysis.Deployments.Select(x => x.ToCountry))
                           .Concat(
                               gameInformation.GetAllCountriesOwnedByPlayer(_player).Where(x => x.IsContinentGateway()));
        }
    }
}