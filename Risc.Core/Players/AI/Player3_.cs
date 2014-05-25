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

        public void Deploy(TurnManager turnManager, int numberOfTroops)
        {
            while (numberOfTroops > 0)
            {
                turnManager.DeployTroops(GetRandomOwnedCountry(turnManager), 1);
                numberOfTroops--;
            }
        }

        public void Attack(TurnManager turnManager)
        {
            var country = GetRandomOwnedCountryThatCanAttack(turnManager);
            var countryToAttack = GetRandomAdjacentCountryToAttack(turnManager, country);

            turnManager.Attack(country, countryToAttack, country.NumberOfTroops - 1);
        }

        public void Move(TurnManager turnManager)
        {
           turnManager.MoveTroops(GetRandomOwnedCountryThatCanAttack(turnManager),
                                                          GetRandomOwnedCountryThatCanAttack(turnManager), 1);
        }

        public int Defend(TurnManager turnManager, List<int> attackRolls, Country countryToDefend)
        {
            return 2;
        }

        private Country GetRandomOwnedCountry(TurnManager turnManager)
        {
            var ownedCountries = turnManager.GetGameInfo().GetAllCountriesOwnedByPlayer(this);
            return ownedCountries[r.Next(0, ownedCountries.Count - 1)];
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
    }
}