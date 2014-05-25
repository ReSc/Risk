using Risk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Risk.Models;
using System.Diagnostics;

namespace Risk.Players
{
    public class Aad : IPlayer
    {
        public string Name
        {
            get { return "Aad"; }
        }

        public string Color
        {
            get { return "YELLOW"; }
        }

        public void Deploy(int numberOfTroops)
        {
            while (numberOfTroops > 0)
            {

                new TurnManager(this).DeployTroops(GetMostImportantCountry(), 1);
                numberOfTroops--;
            }
        }

        private Country GetRandomOwnedCountry()
        {
            var ownedCountries = new GameInformation().GetAllCountriesOwnedByPlayer(this);
            return ownedCountries[r.Next(0, ownedCountries.Count - 1)];
        }

        public void Attack()
        {
            for (var i = 1; i < 250; i++)
            {
                var possibleAttacks = GetAllPossibleAttacks();

                foreach (var possibleAttack in possibleAttacks)
                {
                    possibleAttack.SuccesRate = GetSuccessRate(possibleAttack);
                }

                var myAttack = possibleAttacks.OrderBy(pa => pa.SuccesRate).Last();

                var riskRate = 900;

                if (IsLastEnemyInContinent(myAttack.Victim))
                {
                    riskRate = 400;
                }


                if (myAttack.SuccesRate < riskRate)
                {
                    return;
                }

                new TurnManager(this).Attack(myAttack.MyCountry, myAttack.Victim, myAttack.NumberOfTroops);
            }

        }

        private bool IsLastEnemyInContinent(Country country)
        {

            var victimContinent = country.Continent;

            var info = new GameInformation();

            var myCountriesInContinent = info.GetAllCountriesOwnedByPlayer(this).Count(c => c.Continent == country.Continent);
            var allCountriesInContinent = info.GetAllCountries().Count(c => c.Continent == country.Continent);

            return (allCountriesInContinent - myCountriesInContinent) == 1;

        }

        public void Move()
        {
            for (int i = 0; i < 250; i++)
            {
                var info = new GameInformation();
                foreach (var country in info.GetAllCountriesOwnedByPlayer(this))
                {
                    var surroundingFriendlyCountries = country.AdjacentCountries.ToList<Country>().Where(c => c.Owner == this).Count();

                    if (surroundingFriendlyCountries == country.AdjacentCountries.Count)
                    {
                        while (country.NumberOfTroops > 1)
                        {
                            MoveTroopsToSurroundingCountries(country);
                        }
                    }


                }

            }







        }

        private void MoveTroopsToSurroundingCountries(Country country)
        {
            new TurnManager(this).MoveTroops(country, GetMostImportantSurroundingCountry(country), 1);
        }

        public int Defend(List<int> attackRolls, Country countryToDefend)
        {
            return 2;
        }


        Random r = new Random();

        private Country GetMostImportantCountry()
        {
            return new GameInformation().GetAllCountriesOwnedByPlayer(this).OrderBy(c => GetCountryScore(c)).Last();


        }

        private Country GetMostImportantSurroundingCountry(Country country)
        {
            return country.AdjacentCountries.ToList<Country>().Where(c => c.Owner == this).OrderBy(c => GetCountryScore(c)).Last();


        }

        private Country GetRandomOwnedCountryThatCanAttack()
        {
            var ownedCountries = new GameInformation().GetAllCountriesOwnedByPlayer(this).Where(c => c.NumberOfTroops > 1 &&
                                                                                                c.AdjacentCountries.Any(ac => ac.Owner != c.Owner)).ToList();
            return ownedCountries[r.Next(0, ownedCountries.Count - 1)];
        }

        private Country GetRandomAdjacentCountryToAttack(Country country)
        {
            var adjacentCountries = new GameInformation().GetAdjacentCountriesWithDifferentOwner(country);
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

            double rate = 1000 * ((Convert.ToDouble(myTroops)) / (Convert.ToDouble(targetTroops + 1)));

            return Convert.ToInt16(rate);

        }


        private double GetCountryScore(Country country)
        {
            int NumberOfPotentialTroopsFromContinent = 0;
            int StaticValue = 0;
            int SurroundingFriendlyCountries = 0;
            int SurroundingEnemyCountries = 0;

            int SurroundingFriendlyTroops = 0;
            int SurroundingEnemyTroops = 0;

            var average = AverageTroopsPerCountry();

            if (country.NumberOfTroops > (1.5 * average))
            {
                return 0;
            }

            SurroundingFriendlyCountries = country.AdjacentCountries.ToList<Country>().Where(c => c.Owner == this).Count();
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



        private List<PossibleAttack> GetAllPossibleAttacks()
        {
            var allAttacks = new List<PossibleAttack>();
            foreach (var myCountry in new GameInformation().GetAllCountriesOwnedByPlayer(this))
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

        private class PossibleAttack
        {
            public readonly Country MyCountry;
            public readonly Country Victim;
            public readonly int NumberOfTroops;

            public int SuccesRate;

            public PossibleAttack(Country myCountry, Country victim, int troops)
            {
                MyCountry = myCountry;
                Victim = victim;
                NumberOfTroops = troops;
            }
        }


        private double AverageTroopsPerCountry()
        {
            var info = new GameInformation();
            var totalCountries = 0.0;
            var totalTroops = 0.0;
            info.GetAllCountriesOwnedByPlayer(this).ToList<Country>().ForEach(
                (Country c) =>
                {
                    totalCountries += 1;
                    totalTroops += c.NumberOfTroops;
                });

            return totalTroops / totalCountries;

        }

    }


}
