using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Risk.Models;

namespace Risk.Core
{
    public class DeploymentCounter
    {
        private readonly IPlayer player;
        private readonly GameInformation gameInformation;

        public DeploymentCounter(IPlayer player)
        {
            this.player = player;
            gameInformation = new GameInformation();
        }



        public int GetTroopsToDeploy()
        {
            int troopsToDeploy = gameInformation.GetAllCountriesOwnedByPlayer(player).Count / 3;
            
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