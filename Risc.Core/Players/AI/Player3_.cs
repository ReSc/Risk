using System;
using System.Collections.Generic;
using System.Linq;
using Risk.Core;
using Risk.Models;

namespace Risk.Players
{
    public class Player3 : IPlayer
    {
        private readonly Random r = new Random();

        public string Name
        {
            get { return "player3"; }
        }

        public string Color
        {
            get { return "Blue"; }
        }

        public void Deploy(GameManager gameManager, int numberOfTroops)
        {
            while (numberOfTroops > 0)
            {
                new TurnManager(this, gameManager).DeployTroops(GetRandomOwnedCountry(gameManager), 1);
                numberOfTroops--;
            }
        }

        public void Attack(GameManager gameManager)
        {
            var country = GetRandomOwnedCountryThatCanAttack(gameManager);
            var countryToAttack = GetRandomAdjacentCountryToAttack(gameManager, country);

            new TurnManager(this, gameManager).Attack(country, countryToAttack, country.NumberOfTroops - 1);
        }

        public void Move(GameManager gameManager)
        {
            new TurnManager(this, gameManager).MoveTroops(GetRandomOwnedCountryThatCanAttack(gameManager),
                                                          GetRandomOwnedCountryThatCanAttack(gameManager), 1);
        }

        public int Defend(GameManager gameManager, List<int> attackRolls, Country countryToDefend)
        {
            return 2;
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

        private Country GetRandomAdjacentCountryToAttack(GameManager gameManager, Country country)
        {
            var adjacentCountries =
                new GameInformation(gameManager).GetAdjacentCountriesWithDifferentOwner(country);
            return adjacentCountries[r.Next(0, adjacentCountries.Count - 1)];
        }
    }
}