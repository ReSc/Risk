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

        public void Deploy(GameManager gameManager, int numberOfTroops)
        {
            while (numberOfTroops > 0)
            {
                new TurnManager(this, gameManager).DeployTroops(GetMostImportantCountry(gameManager), 1);
                numberOfTroops--;
            }
        }

        public void Attack(GameManager gameManager)
        {
            for (var i = 1; i < 250; i++)
            {
                var possibleAttacks = GetAllPossibleAttacks(gameManager);

                foreach (var possibleAttack in possibleAttacks)
                {
                    possibleAttack.SuccesRate = GetSuccessRate(possibleAttack);
                }

                var myAttack = possibleAttacks.OrderBy(pa => pa.SuccesRate).Last();

                var riskRate = 900;

                if (IsLastEnemyInContinent(myAttack.Victim, gameManager))
                {
                    riskRate = 400;
                }


                if (myAttack.SuccesRate < riskRate)
                {
                    return;
                }

                new TurnManager(this, gameManager).Attack(myAttack.MyCountry, myAttack.Victim, myAttack.NumberOfTroops);
            }
        }

        public void Move(GameManager gameManager)
        {
            for (var i = 0; i < 250; i++)
            {
                var info = new GameInformation(gameManager);
                foreach (var country in info.GetAllCountriesOwnedByPlayer(this))
                {
                    var surroundingFriendlyCountries =
                        country.AdjacentCountries.ToList().Where(c => c.Owner == this).Count();

                    if (surroundingFriendlyCountries == country.AdjacentCountries.Count)
                    {
                        while (country.NumberOfTroops > 1)
                        {
                            MoveTroopsToSurroundingCountries(country, gameManager);
                        }
                    }
                }
            }
        }

        public int Defend(GameManager gameManager, List<int> attackRolls, Country countryToDefend)
        {
            return 2;
        }

        private bool IsLastEnemyInContinent(Country country, GameManager gameManager)
        {
            var victimContinent = country.Continent;

            var info = new GameInformation(gameManager);

            var myCountriesInContinent =
                info.GetAllCountriesOwnedByPlayer(this).Count(c => c.Continent == country.Continent);
            var allCountriesInContinent = info.GetAllCountries().Count(c => c.Continent == country.Continent);

            return (allCountriesInContinent - myCountriesInContinent) == 1;
        }

        private void MoveTroopsToSurroundingCountries(Country country, GameManager gameManager)
        {
            new TurnManager(this, gameManager).MoveTroops(country,
                                                          GetMostImportantSurroundingCountry(gameManager, country), 1);
        }


        private Country GetMostImportantCountry(GameManager gameManager)
        {
            return
                new GameInformation(gameManager).GetAllCountriesOwnedByPlayer(this)
                                                .OrderBy(c => GetCountryScore(gameManager, c))
                                                .Last();
        }

        private Country GetMostImportantSurroundingCountry(GameManager gameManager, Country country)
        {
            return
                country.AdjacentCountries.ToList()
                       .Where(c => c.Owner == this)
                       .OrderBy(c => GetCountryScore(gameManager, c))
                       .Last();
        }

        private Country GetRandomOwnedCountryThatCanAttack(GameManager gameManager)
        {
            var ownedCountries =
                new GameInformation(gameManager).GetAllCountriesOwnedByPlayer(this).Where(c => c.NumberOfTroops > 1 &&
                                                                                               c.AdjacentCountries.Any(
                                                                                                   ac =>
                                                                                                   ac.Owner != c.Owner))
                                                .ToList();
            return ownedCountries[r.Next(0, ownedCountries.Count - 1)];
        }

        private Country GetRandomAdjacentCountryToAttack(Country country, GameManager gameManager)
        {
            var adjacentCountries =
                new GameInformation(gameManager).GetAdjacentCountriesWithDifferentOwner(country);
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

            var rate = 1000*((Convert.ToDouble(myTroops))/(Convert.ToDouble(targetTroops + 1)));

            return Convert.ToInt16(rate);
        }


        private double GetCountryScore(GameManager gameManager, Country country)
        {
            var NumberOfPotentialTroopsFromContinent = 0;
            var StaticValue = 0;
            var SurroundingFriendlyCountries = 0;
            var SurroundingEnemyCountries = 0;

            var SurroundingFriendlyTroops = 0;
            var SurroundingEnemyTroops = 0;

            var average = AverageTroopsPerCountry(gameManager);

            if (country.NumberOfTroops > (1.5*average))
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


            var rate = NumberOfPotentialTroopsFromContinent*1 +
                       StaticValue*1 +
                       SurroundingEnemyTroops*1 -
                       SurroundingFriendlyTroops*1 +
                       SurroundingEnemyCountries*1 -
                       SurroundingFriendlyCountries*1 +
                       belowAverage*2;

            return rate;
        }


        private List<PossibleAttack> GetAllPossibleAttacks(GameManager gameManager)
        {
            var allAttacks = new List<PossibleAttack>();
            foreach (var myCountry in new GameInformation(gameManager).GetAllCountriesOwnedByPlayer(this))
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


        private double AverageTroopsPerCountry(GameManager gameManager)
        {
            var info = new GameInformation(gameManager);
            var totalCountries = 0.0;
            var totalTroops = 0.0;
            info.GetAllCountriesOwnedByPlayer(this).ToList().ForEach(
                (Country c) =>
                    {
                        totalCountries += 1;
                        totalTroops += c.NumberOfTroops;
                    });

            return totalTroops/totalCountries;
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