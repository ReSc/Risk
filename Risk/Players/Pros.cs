using Risk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Risk.Models;

namespace Risk.Players
{
    public class Pros : IPlayer
    {
        public string Name
        {
            get { return "Pro's"; }
        }

        public string Color
        {
            get { return "RED"; }
        }



        /*
         
         DONT LOOK AT OUR CODE PLS
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         

         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         */



        public void Deploy(int numberOfTroops)
        {


            if (numberOfTroops == 2)
            {
                List<Country> tempList = new GameInformation().GetAllCountriesOwnedByPlayer(this);
                List<Country> Australia = tempList.Where(p => p.Continent == EContinent.Australia).ToList();
                List<Country> SouthAmerica = tempList.Where(p => p.Continent == EContinent.SouthAmerica).ToList();
                List<Country> Africa = tempList.Where(p => p.Continent == EContinent.Africa).ToList();
                List<Country> NorthAmerica = tempList.Where(p => p.Continent == EContinent.NorthAmerica).ToList();
                List<Country> Europe = tempList.Where(p => p.Continent == EContinent.Europe).ToList();
                List<Country> Asia = tempList.Where(p => p.Continent == EContinent.Asia).ToList();
                int AustraliaCount = Australia.Count;
                if (Australia.Count >= 2)
                {
                    if (AustraliaCount == 4)
                    {
                        var AustraliaBottleNeck = tempList.Where(p => p.Continent == EContinent.Australia && p.Number == 4).Single();
                        List<Country> i = AustraliaBottleNeck.AdjacentCountries.Where(p => p.Owner != this).ToList();
                        while (AustraliaBottleNeck.NumberOfTroops < 10 && numberOfTroops > 0 && i.Count > 0)
                        {

                            new TurnManager(this).DeployTroops(AustraliaBottleNeck, 1);
                            numberOfTroops--;

                        }
                    }
                    else
                    {
                        var Random = new Random();
                        var AustraliaBottleNeck = tempList.Where(p => p.Continent == EContinent.Australia);
                        if (AustraliaBottleNeck != null)
                        {
                            int index = Random.Next(Australia.Count);
                            Country country = Australia[index];
                            List<Country> i = country.AdjacentCountries.Where(p => p.Owner != this).ToList();
                            while (country.NumberOfTroops < 10 && numberOfTroops > 0 && i.Count > 0)
                            {

                                new TurnManager(this).DeployTroops(country, 1);
                                numberOfTroops--;

                            }
                        }
                    }
                }
                if (SouthAmerica.Count >= 2)
                {

                    var Random = new Random();
                    var SouthAmericaBottleNeck = tempList.Where(p => p.Continent == EContinent.SouthAmerica);
                    if (SouthAmericaBottleNeck != null)
                    {
                        int index = Random.Next(SouthAmerica.Count);
                        Country country = SouthAmerica[index];
                        List<Country> i = country.AdjacentCountries.Where(p => p.Owner != this).ToList();
                        while (country.NumberOfTroops < 10 && numberOfTroops > 0 && i.Count > 0)
                        {


                            new TurnManager(this).DeployTroops(country, 1);
                            numberOfTroops--;

                        }
                    }

                }

                if (Africa.Count >= 4)
                {

                    var Random = new Random();
                    var AfricaBottleNeck = tempList.Where(p => p.Continent == EContinent.Africa);
                    if (AfricaBottleNeck != null)
                    {
                        int index = Random.Next(Africa.Count);
                        Country country = Africa[index];
                        List<Country> i = country.AdjacentCountries.Where(p => p.Owner != this).ToList();
                        while (country.NumberOfTroops < 10 && numberOfTroops > 0 && i.Count > 0)
                        {

                            new TurnManager(this).DeployTroops(country, 1);
                            numberOfTroops--;

                        }
                    }
                }
                if (NorthAmerica.Count >= 5)
                {
                    var Random = new Random();
                    var AfricaBottleNeck = tempList.Where(p => p.Continent == EContinent.NorthAmerica);
                    if (AfricaBottleNeck != null)
                    {
                        int index = Random.Next(NorthAmerica.Count);
                        Country country = NorthAmerica[index];
                        List<Country> i = country.AdjacentCountries.Where(p => p.Owner != this).ToList();
                        while (country.NumberOfTroops < 10 && numberOfTroops > 0 && i.Count > 0)
                        {

                            new TurnManager(this).DeployTroops(country, 1);
                            numberOfTroops--;

                        }
                    }
                }
                if (Europe.Count >= 7)
                {
                    var Random = new Random();
                    var AfricaBottleNeck = tempList.Where(p => p.Continent == EContinent.Europe);
                    if (AfricaBottleNeck != null)
                    {
                        int index = Random.Next(Europe.Count);
                        Country country = Europe[index];
                        List<Country> i = country.AdjacentCountries.Where(p => p.Owner != this).ToList();
                        while (country.NumberOfTroops < 10 && numberOfTroops > 0 && i.Count > 0)
                        {

                            new TurnManager(this).DeployTroops(country, 1);
                            numberOfTroops--;
                        }

                    }
                }

                if (Asia.Count >= 8)
                {
                    var Random = new Random();
                    var AfricaBottleNeck = tempList.Where(p => p.Continent == EContinent.Asia);
                    if (AfricaBottleNeck != null)
                    {
                        int index = Random.Next(Asia.Count);
                        Country country = Asia[index];
                        List<Country> i = country.AdjacentCountries.Where(p => p.Owner != this).ToList();
                        while (country.NumberOfTroops < 10 && numberOfTroops > 0 && i.Count > 0)
                        {
                            new TurnManager(this).DeployTroops(country, 1);
                            numberOfTroops--;

                        }
                    }
                }


                while (numberOfTroops > 0)
                {
                    var target = GetRandomOwnedCountry();

                    List<Country> i = target.AdjacentCountries.Where(p => p.Owner != this).ToList();
                    if (i.Count > 0)
                    {
                        if (target.NumberOfTroops < 7)
                        {
                            int z = 0;
                            while (numberOfTroops > 0)
                            {

                                new TurnManager(this).DeployTroops(GetRandomOwnedCountry(), 1);
                                z++;
                                numberOfTroops--;
                                if (z > 30)
                                {

                                    break;
                                }
                            }
                        }

                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                while (numberOfTroops > 0)
                {


                    new TurnManager(this).DeployTroops(GetRandomOwnedCountry(), 1);


                    numberOfTroops--;
                }
            }
            else
            {
                List<Country> myCountries = new GameInformation().GetAllCountriesOwnedByPlayer(this);
                List<Country> potential = new List<Country>();
                foreach (var country in myCountries)
                {
                    if (country.AdjacentCountries.Where(p => p.Owner != this).ToList().Count > 0)
                    {
                        potential.Add(country);
                    }
                }
                List<Country> tempList = potential.OrderBy(p => p.NumberOfTroops).ToList();
                foreach (var countryy in tempList)
                {

                    while (countryy.NumberOfTroops < 10 && numberOfTroops > 0)
                    {

                        new TurnManager(this).DeployTroops(countryy, 1);
                        numberOfTroops--;
                    }



                }
                if (numberOfTroops > 0)
                {

                    foreach (var countryy in tempList)
                    {

                        while (numberOfTroops > 0)
                        {

                            new TurnManager(this).DeployTroops(countryy, 1);
                            numberOfTroops--;
                        }



                    }
                }
            }


        }

        public void Attack()
        {
            //var country = GetRandomOwnedCountryThatCanAttack();
            //var countryToAttack = GetRandomAdjacentCountryToAttack(country);

            var repeat = 0;
            var timeout = 0;

            do
            {

                List<Country> tempList = new GameInformation().GetAllCountriesOwnedByPlayer(this);
                foreach (var country in tempList)
                {
                    var adjacentCountryList = country.AdjacentCountries.Where(p => p.Owner != this);

                    foreach (var countryToAttack in adjacentCountryList)
                    {
                        if (countryToAttack.NumberOfTroops < country.NumberOfTroops && country.NumberOfTroops > 3)
                        {
                            new TurnManager(this).Attack(country, countryToAttack, country.NumberOfTroops - 1);
                            repeat++;
                        }

                    }
                }

                timeout++;
            }
            while (repeat < 2 && timeout < 200);


        }

        public void Move()
        {
            var movesLeft = 7;

            List<Country> tempList = new GameInformation().GetAllCountriesOwnedByPlayer(this);

            foreach (var country in tempList)
            {
                if (country.NumberOfTroops > 1)
                {
                    var hasHostile = false;
                    List<Country> checkForHostileCountries = country.AdjacentCountries.Where(p => p.Owner != this).ToList();

                    if (checkForHostileCountries.Count > 0)
                    {
                        hasHostile = true;
                    }

                    if (hasHostile == false)
                    {
                        List<Country> adjacentCountryListOwned = country.AdjacentCountries.Where(p => p.Owner == this).ToList();


                        foreach (var countryToMove in adjacentCountryListOwned)
                        {

                            List<Country> adjacentCountryListEnemey = countryToMove.AdjacentCountries.Where(p => p.Owner != this).ToList();
                            var hasHostile2 = false;
                            if (adjacentCountryListEnemey.Count > 0)
                            {
                                hasHostile2 = true;
                            }

                            if (hasHostile2)
                            {
                                if (country.NumberOfTroops - 1 > 0 && movesLeft > 0)
                                {
                                    if (movesLeft > (country.NumberOfTroops - 1))
                                    {
                                        new TurnManager(this).MoveTroops(country, countryToMove, country.NumberOfTroops - 1);
                                        movesLeft = movesLeft - (country.NumberOfTroops - 1);
                                    }
                                    else
                                    {
                                        new TurnManager(this).MoveTroops(country, countryToMove, movesLeft);
                                        movesLeft = 0;
                                    }


                                }
                            }
                            else
                            {
                                List<Country> adjacentCountryListOwned2 = countryToMove.AdjacentCountries.Where(p => p.Owner == this).ToList();


                                foreach (var countryToMove2 in adjacentCountryListOwned2)
                                {

                                    List<Country> adjacentCountryListEnemey2 = countryToMove2.AdjacentCountries.Where(p => p.Owner != this).ToList();
                                    var hasHostile3 = false;
                                    if (adjacentCountryListEnemey2.Count > 0)
                                    {
                                        hasHostile3 = true;
                                    }

                                    if (hasHostile3)
                                    {
                                        if (country.NumberOfTroops - 1 > 0 && movesLeft > 0)
                                        {
                                            if (movesLeft > (country.NumberOfTroops - 1))
                                            {
                                                new TurnManager(this).MoveTroops(country, countryToMove, country.NumberOfTroops - 1);
                                                movesLeft = movesLeft - (country.NumberOfTroops - 1);
                                            }
                                            else
                                            {
                                                new TurnManager(this).MoveTroops(country, countryToMove, movesLeft);
                                                movesLeft = 0;
                                            }

                                        }
                                    }
                                    else
                                    {

                                        //hmm

                                    }



                                }

                            }



                        }


                    }
                }
            }
        }

        public int Defend(List<int> attackRolls, Country countryToDefend)
        {

            if (attackRolls.Count == 3)
            {
                int dangerlevel = 0;

                if (attackRolls[0] >= 5)
                {
                    dangerlevel++;
                }

                if (attackRolls[1] >= 5)
                {
                    dangerlevel++;
                }

                if (attackRolls[2] >= 5)
                {
                    dangerlevel++;
                }

                if (dangerlevel >= 2)
                {
                    return 1;
                }

                return 2;

            }
            else
            {

                int dangerlevel = 0;

                if (attackRolls[0] >= 5)
                {
                    dangerlevel++;
                }

                if (attackRolls[1] >= 5)
                {
                    dangerlevel++;
                }

                if (dangerlevel >= 2)
                {
                    return 1;
                }

                return 2;


            }




        }


        Random r = new Random();

        private Country GetRandomOwnedCountry()
        {
            var ownedCountries = new GameInformation().GetAllCountriesOwnedByPlayer(this);
            return ownedCountries[r.Next(0, ownedCountries.Count - 1)];
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


    }
}

