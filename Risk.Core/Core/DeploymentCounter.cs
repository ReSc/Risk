using System.Linq;
using Risk.Models;

namespace Risk.Core
{
    public class DeploymentCounter
    {
        private readonly GameInformation gameInformation;
        private readonly IPlayer player;

        public DeploymentCounter(IPlayer player, GameInformation gameInformation)
        {
            this.player = player;
            this .gameInformation = gameInformation;
        }


        public int GetTroopsToDeploy()
        {
            var troopsToDeploy = gameInformation.GetAllCountriesOwnedByPlayer(player).Count/3;

            if (PlayerOwnsAllCountriesOfGivenContinent(EContinent.Australia)) troopsToDeploy += 2;
            if (PlayerOwnsAllCountriesOfGivenContinent(EContinent.SouthAmerica)) troopsToDeploy += 2;
            if (PlayerOwnsAllCountriesOfGivenContinent(EContinent.Africa)) troopsToDeploy += 3;
            if (PlayerOwnsAllCountriesOfGivenContinent(EContinent.Europe)) troopsToDeploy += 5;
            if (PlayerOwnsAllCountriesOfGivenContinent(EContinent.NorthAmerica)) troopsToDeploy += 5;
            if (PlayerOwnsAllCountriesOfGivenContinent(EContinent.Asia)) troopsToDeploy += 7;

            return troopsToDeploy >= 3 ? troopsToDeploy : 3;
        }

        private bool PlayerOwnsAllCountriesOfGivenContinent(EContinent continent)
        {
            return gameInformation.GetAllCountries().Where(c => c.Continent == continent).All(c => c.Owner == player);
        }
    }
}