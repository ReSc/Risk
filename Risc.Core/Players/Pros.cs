using System;
using System.Collections.Generic;
using System.Linq;
using Risk.Core;
using Risk.Models;

namespace Risk.Players
{
    public class Pros : IPlayer
    {
        private readonly Random r = new Random();

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


        public void Deploy(GameManager gameManager, int numberOfTroops)
        {
            if (numberOfTroops == 2)
            {
                var tempList = new GameInformation(gameManager).GetAllCountriesOwnedByPlayer(this);
                var Australia = tempList.Where(p => p.Continent == EContinent.Australia).ToList();
                var SouthAmerica = tempList.Where(p => p.Continent == EContinent.SouthAmerica).ToList();
                var Africa = tempList.Where(p => p.Continent == EContinent.Africa).ToList();
                var NorthAmerica = tempList.Where(p => p.Continent == EContinent.NorthAmerica).ToList();
                var Europe = tempList.Where(p => p.Continent == EContinent.Europe).ToList();
                var Asia = tempList.Where(p => p.Continent == EContinent.Asia).ToList();
                var AustraliaCount = Australia.Count;
                if (Australia.Count >= 2)
                {
                    if (AustraliaCount == 4)
                    {
                        var AustraliaBottleNeck =
                            tempList.Where(p => p.Continent == EContinent.Australia && p.Number == 4).Single();
                        var i = AustraliaBottleNeck.AdjacentCountries.Where(p => p.Owner != this).ToList();
                        while (AustraliaBottleNeck.NumberOfTroops < 10 && numberOfTroops > 0 && i.Count > 0)
                        {
                            new TurnManager(this, gameManager).DeployTroops(AustraliaBottleNeck, 1);
                            numberOfTroops--;
                        }
                    }
                    else
                    {
                        var Random = new Random();
                        var AustraliaBottleNeck =
                            tempList.Where(p => p.Continent == EContinent.Australia);
                        if (AustraliaBottleNeck != null)
                        {
                            var index = Random.Next(Australia.Count);
                            var country = Australia[index];
                            var i = country.AdjacentCountries.Where(p => p.Owner != this).ToList();
                            while (country.NumberOfTroops < 10 && numberOfTroops > 0 && i.Count > 0)
                            {
                                new TurnManager(this, gameManager).DeployTroops(country, 1);
                                numberOfTroops--;
                            }
                        }
                    }
                }
                if (SouthAmerica.Count >= 2)
                {
                    var Random = new Random();
                    var SouthAmericaBottleNeck =
                        tempList.Where(p => p.Continent == EContinent.SouthAmerica);
                    if (SouthAmericaBottleNeck != null)
                    {
                        var index = Random.Next(SouthAmerica.Count);
                        var country = SouthAmerica[index];
                        var i = country.AdjacentCountries.Where(p => p.Owner != this).ToList();
                        while (country.NumberOfTroops < 10 && numberOfTroops > 0 && i.Count > 0)
                        {
                            new TurnManager(this, gameManager).DeployTroops(country, 1);
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
                        var index = Random.Next(Africa.Count);
                        var country = Africa[index];
                        var i = country.AdjacentCountries.Where(p => p.Owner != this).ToList();
                        while (country.NumberOfTroops < 10 && numberOfTroops > 0 && i.Count > 0)
                        {
                            new TurnManager(this, gameManager).DeployTroops(country, 1);
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
                        var index = Random.Next(NorthAmerica.Count);
                        var country = NorthAmerica[index];
                        var i = country.AdjacentCountries.Where(p => p.Owner != this).ToList();
                        while (country.NumberOfTroops < 10 && numberOfTroops > 0 && i.Count > 0)
                        {
                            new TurnManager(this, gameManager).DeployTroops(country, 1);
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
                        var index = Random.Next(Europe.Count);
                        var country = Europe[index];
                        var i = country.AdjacentCountries.Where(p => p.Owner != this).ToList();
                        while (country.NumberOfTroops < 10 && numberOfTroops > 0 && i.Count > 0)
                        {
                            new TurnManager(this, gameManager).DeployTroops(country, 1);
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
                        var index = Random.Next(Asia.Count);
                        var country = Asia[index];
                        var i = country.AdjacentCountries.Where(p => p.Owner != this).ToList();
                        while (country.NumberOfTroops < 10 && numberOfTroops > 0 && i.Count > 0)
                        {
                            new TurnManager(this, gameManager).DeployTroops(country, 1);
                            numberOfTroops--;
                        }
                    }
                }


                while (numberOfTroops > 0)
                {
                    var target = GetRandomOwnedCountry(gameManager);

                    var i = target.AdjacentCountries.Where(p => p.Owner != this).ToList();
                    if (i.Count > 0)
                    {
                        if (target.NumberOfTroops < 7)
                        {
                            var z = 0;
                            while (numberOfTroops > 0)
                            {
                                new TurnManager(this, gameManager).DeployTroops(GetRandomOwnedCountry(gameManager), 1);
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
                    new TurnManager(this, gameManager).DeployTroops(GetRandomOwnedCountry(gameManager), 1);


                    numberOfTroops--;
                }
            }
            else
            {
                var myCountries = new GameInformation(gameManager).GetAllCountriesOwnedByPlayer(this);
                var potential = new List<Country>();
                foreach (var country in myCountries)
                {
                    if (country.AdjacentCountries.Where(p => p.Owner != this).ToList().Count > 0)
                    {
                        potential.Add(country);
                    }
                }
                var tempList = potential.OrderBy(p => p.NumberOfTroops).ToList();
                foreach (var countryy in tempList)
                {
                    while (countryy.NumberOfTroops < 10 && numberOfTroops > 0)
                    {
                        new TurnManager(this, gameManager).DeployTroops(countryy, 1);
                        numberOfTroops--;
                    }
                }
                if (numberOfTroops > 0)
                {
                    foreach (var countryy in tempList)
                    {
                        while (numberOfTroops > 0)
                        {
                            new TurnManager(this, gameManager).DeployTroops(countryy, 1);
                            numberOfTroops--;
                        }
                    }
                }
            }
        }

        public void Attack(GameManager gameManager)
        {
            //var country = GetRandomOwnedCountryThatCanAttack();
            //var countryToAttack = GetRandomAdjacentCountryToAttack(country);

            var repeat = 0;
            var timeout = 0;

            do
            {
                var tempList = new GameInformation(gameManager).GetAllCountriesOwnedByPlayer(this);
                foreach (var country in tempList)
                {
                    var adjacentCountryList = country.AdjacentCountries.Where(p => p.Owner != this);

                    foreach (var countryToAttack in adjacentCountryList)
                    {
                        if (countryToAttack.NumberOfTroops < country.NumberOfTroops && country.NumberOfTroops > 3)
                        {
                            new TurnManager(this, gameManager).Attack(country, countryToAttack,
                                                                      country.NumberOfTroops - 1);
                            repeat++;
                        }
                    }
                }

                timeout++;
            } while (repeat < 2 && timeout < 200);
        }

        public void Move(GameManager gameManager)
        {
            var movesLeft = 7;

            var tempList = new GameInformation(gameManager).GetAllCountriesOwnedByPlayer(this);

            foreach (var country in tempList)
            {
                if (country.NumberOfTroops > 1)
                {
                    var hasHostile = false;
                    var checkForHostileCountries =
                        country.AdjacentCountries.Where(p => p.Owner != this).ToList();

                    if (checkForHostileCountries.Count > 0)
                    {
                        hasHostile = true;
                    }

                    if (hasHostile == false)
                    {
                        var adjacentCountryListOwned =
                            country.AdjacentCountries.Where(p => p.Owner == this).ToList();


                        foreach (var countryToMove in adjacentCountryListOwned)
                        {
                            var adjacentCountryListEnemey =
                                countryToMove.AdjacentCountries.Where(p => p.Owner != this).ToList();
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
                                        new TurnManager(this, gameManager).MoveTroops(country, countryToMove,
                                                                                      country.NumberOfTroops - 1);
                                        movesLeft = movesLeft - (country.NumberOfTroops - 1);
                                    }
                                    else
                                    {
                                        new TurnManager(this, gameManager).MoveTroops(country, countryToMove, movesLeft);
                                        movesLeft = 0;
                                    }
                                }
                            }
                            else
                            {
                                var adjacentCountryListOwned2 =
                                    countryToMove.AdjacentCountries.Where(p => p.Owner == this).ToList();


                                foreach (var countryToMove2 in adjacentCountryListOwned2)
                                {
                                    var adjacentCountryListEnemey2 =
                                        countryToMove2.AdjacentCountries.Where(p => p.Owner != this).ToList();
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
                                                new TurnManager(this, gameManager).MoveTroops(country, countryToMove,
                                                                                              country.NumberOfTroops - 1);
                                                movesLeft = movesLeft - (country.NumberOfTroops - 1);
                                            }
                                            else
                                            {
                                                new TurnManager(this, gameManager).MoveTroops(country, countryToMove,
                                                                                              movesLeft);
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

        public int Defend(GameManager gameManager, List<int> attackRolls, Country countryToDefend)
        {
            if (attackRolls.Count == 3)
            {
                var dangerlevel = 0;

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
                var dangerlevel = 0;

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


        private Country GetRandomOwnedCountry(GameManager gameManager)
        {
            var ownedCountries = new GameInformation(gameManager).GetAllCountriesOwnedByPlayer(this);
            return ownedCountries[r.Next(0, ownedCountries.Count - 1)];
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
    }
}