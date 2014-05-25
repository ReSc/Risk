using System;
using System.Collections.Generic;
using System.Linq;
using Risk.Core;
using Risk.Models;

namespace Risk.Players
{
    public class Aad : IPlayer
    {
        private readonly Random r = new Random();

        public string Name
        {
            get { return "Aad"; }
        }

        public string Color
        {
            get { return "YELLOW"; }
        }

        public void Deploy(TurnManager turnManager, int numberOfTroops)
        {
            while (numberOfTroops > 0)
            {
                turnManager.DeployTroops(GetMostImportantCountry(turnManager), 1);
                numberOfTroops--;
            }
        }

        public void Attack(TurnManager turnManager)
        {
            for (var i = 1; i < 250; i++)
            {
                var possibleAttacks = GetAllPossibleAttacks(turnManager);

                foreach (var possibleAttack in possibleAttacks)
                {
                    possibleAttack.SuccesRate = GetSuccessRate(possibleAttack);
                }

                var myAttack = possibleAttacks.OrderBy(pa => pa.SuccesRate).Last();

                var riskRate = 900;

                if (IsLastEnemyInContinent(turnManager, myAttack.Victim))
                {
                    riskRate = 400;
                }


                if (myAttack.SuccesRate < riskRate)
                {
                    return;
                }

                turnManager.Attack(myAttack.MyCountry, myAttack.Victim, myAttack.NumberOfTroops);
            }
        }

        public void Move(TurnManager turnManager)
        {
            for (var i = 0; i < 250; i++)
            {
                var info = turnManager.GetGameInfo();
                foreach (var country in info.GetAllCountriesOwnedByPlayer(this))
                {
                    var surroundingFriendlyCountries =
                        country.AdjacentCountries.ToList().Where(c => c.Owner == this).Count();

                    if (surroundingFriendlyCountries == country.AdjacentCountries.Count)
                    {
                        while (country.NumberOfTroops > 1)
                        {
                            MoveTroopsToSurroundingCountries(turnManager, country);
                        }
                    }
                }
            }
        }

        public int Defend(TurnManager turnManager, List<int> attackRolls, Country countryToDefend)
        {
            return 2;
        }

        private bool IsLastEnemyInContinent(TurnManager turnManager, Country country)
        {
            var victimContinent = country.Continent;

            var info = turnManager.GetGameInfo();

            var myCountriesInContinent =
                info.GetAllCountriesOwnedByPlayer(this).Count(c => c.Continent == country.Continent);
            var allCountriesInContinent = info.GetAllCountries().Count(c => c.Continent == country.Continent);

            return (allCountriesInContinent - myCountriesInContinent) == 1;
        }

        private void MoveTroopsToSurroundingCountries(TurnManager turnManager, Country country)
        {
            turnManager.MoveTroops(country, GetMostImportantSurroundingCountry(turnManager, country), 1);
        }


        private Country GetMostImportantCountry(TurnManager turnManager)
        {
            return turnManager.GetGameInfo().GetAllCountriesOwnedByPlayer(this)
                                                .OrderBy(c => GetCountryScore(turnManager, c))
                                                .Last();
        }

        private Country GetMostImportantSurroundingCountry(TurnManager turnManager, Country country)
        {
            return
                country.AdjacentCountries.ToList()
                       .Where(c => c.Owner == this)
                       .OrderBy(c => GetCountryScore(turnManager, c))
                       .Last();
        }

        private Country GetRandomOwnedCountryThatCanAttack(TurnManager turnManager)
        {
            var ownedCountries =
                turnManager.GetGameInfo().GetAllCountriesOwnedByPlayer(this).Where(c => c.NumberOfTroops > 1 &&
                                                                                               c.AdjacentCountries.Any(
                                                                                                   ac =>
                                                                                                   ac.Owner != c.Owner))
                                                .ToList();
            return ownedCountries[r.Next(0, ownedCountries.Count - 1)];
        }

        private Country GetRandomAdjacentCountryToAttack(TurnManager turnManager, Country country)
        {
            var adjacentCountries =
                turnManager.GetGameInfo().GetAdjacentCountriesWithDifferentOwner(country);
            return adjacentCountries[r.Next(0, adjacentCountries.Count - 1)];
        }


        private int GetSuccessRate(PossibleAttack possibleAttack)
        {
            var myTroops = possibleAttack.NumberOfTroops;
            var targetTroops = possibleAttack.Victim.NumberOfTroops;

            if (targetTroops < 1)
                targetTroops = 1;
            if (myTroops < 1)
                myTroops = 1;

            var rate = 1000 * ((Convert.ToDouble(myTroops)) / (Convert.ToDouble(targetTroops + 1)));

            return Convert.ToInt16(rate);
        }


        private double GetCountryScore(TurnManager turnManager, Country country)
        {
            var NumberOfPotentialTroopsFromContinent = 0;
            var StaticValue = 0;
            var SurroundingFriendlyCountries = 0;
            var SurroundingEnemyCountries = 0;

            var SurroundingFriendlyTroops = 0;
            var SurroundingEnemyTroops = 0;

            var average = AverageTroopsPerCountry(turnManager);

            if (country.NumberOfTroops > (1.5 * average))
            {
                return 0;
            }

            SurroundingFriendlyCountries = country.AdjacentCountries.ToList().Where(c => c.Owner == this).Count();
            SurroundingEnemyCountries = country.AdjacentCountries.Count - SurroundingFriendlyCountries;

            if (SurroundingFriendlyCountries == country.AdjacentCountries.Count)
            {
                return 0;
            }

            switch (country.Continent)
            {
                case EContinent.NorthAmerica:
                    NumberOfPotentialTroopsFromContinent = 4;
                    break;
                case EContinent.SouthAmerica:
                    NumberOfPotentialTroopsFromContinent = 4;
                    break;
                case EContinent.Africa:
                    NumberOfPotentialTroopsFromContinent = 4;
                    break;
                case EContinent.Europe:
                    NumberOfPotentialTroopsFromContinent = 4;
                    break;
                case EContinent.Asia:
                    NumberOfPotentialTroopsFromContinent = 4;
                    break;
                case EContinent.Australia:
                    NumberOfPotentialTroopsFromContinent = 3;
                    break;
                default:
                    break;
            }


            var belowAverage = average - country.NumberOfTroops;


            var rate = NumberOfPotentialTroopsFromContinent * 1 +
                       StaticValue * 1 +
                       SurroundingEnemyTroops * 1 -
                       SurroundingFriendlyTroops * 1 +
                       SurroundingEnemyCountries * 1 -
                       SurroundingFriendlyCountries * 1 +
                       belowAverage * 2;

            return rate;
        }


        private List<PossibleAttack> GetAllPossibleAttacks(TurnManager turnManager)
        {
            var allAttacks = new List<PossibleAttack>();
            foreach (var myCountry in turnManager.GetGameInfo().GetAllCountriesOwnedByPlayer(this))
            {
                foreach (var victim in myCountry.AdjacentCountries.Where(v => v.Owner != this))
                {
                    for (var i = 1; i < myCountry.NumberOfTroops - 1; i++)
                    {
                        allAttacks.Add(new PossibleAttack(myCountry, victim, i));
                    }
                }
            }
            return allAttacks;
        }


        private double AverageTroopsPerCountry(TurnManager turnManager)
        {
            var info = turnManager.GetGameInfo();
            var totalCountries = 0.0;
            var totalTroops = 0.0;
            info.GetAllCountriesOwnedByPlayer(this).ToList().ForEach(
                (Country c) =>
                {
                    totalCountries += 1;
                    totalTroops += c.NumberOfTroops;
                });

            return totalTroops / totalCountries;
        }

        private class PossibleAttack
        {
            public readonly Country MyCountry;
            public readonly int NumberOfTroops;
            public readonly Country Victim;

            public int SuccesRate;

            public PossibleAttack(Country myCountry, Country victim, int troops)
            {
                MyCountry = myCountry;
                Victim = victim;
                NumberOfTroops = troops;
            }
        }
    }
}