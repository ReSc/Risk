using Risk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Risk.Models;

namespace Risk.Players
{
    public class Player3 : IPlayer
    {
        public string Name
        {
            get { return "player3"; }
        }

        public string Color
        {
            get { return "Blue"; }
        }

        public void Deploy(int numberOfTroops)
        {
            while (numberOfTroops > 0)
            {
                new TurnManager(this).DeployTroops(GetRandomOwnedCountry(), 1);
                numberOfTroops--;
            }
        }

        public void Attack()
        {
            var country = GetRandomOwnedCountryThatCanAttack();
            var countryToAttack = GetRandomAdjacentCountryToAttack(country);

            new TurnManager(this).Attack(country, countryToAttack, country.NumberOfTroops - 1);
        }

        public void Move()
        {
            new TurnManager(this).MoveTroops(GetRandomOwnedCountryThatCanAttack(), GetRandomOwnedCountryThatCanAttack(), 1);
        }

        public int Defend(List<int> attackRolls, Country countryToDefend)
        {
            return 2;
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