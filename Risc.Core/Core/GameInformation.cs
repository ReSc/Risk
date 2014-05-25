using System.Collections.Generic;
using System.Linq;
using Risk.Models;

namespace Risk.Core
{
    public class GameInformation
    {
        private readonly GameManager gameManager;

        public GameInformation(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public List<Country> GetAllCountries()
        {
            return gameManager.Countries;
        }

        public List<Country> GetAllCountriesOwnedByPlayer(IPlayer player)
        {
            return gameManager.Countries.Where(country => country.Owner == player).ToList();
        }

        public List<Country> GetAdjacentCountriesWithDifferentOwner(Country country)
        {
            return country.AdjacentCountries.Where(adjacentCountry => adjacentCountry.Owner != country.Owner).ToList();
        }

        public List<Country> GetAdjacentCountriesWithSameOwner(Country country)
        {
            return country.AdjacentCountries.Where(adjacentCountry => adjacentCountry.Owner == country.Owner).ToList();
        }

        public int GetNumberOfUnitsToDeploy(IPlayer player)
        {
            return new DeploymentCounter(player, this).GetTroopsToDeploy();
        }
    }
}