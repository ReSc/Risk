using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Risk.Core;

namespace Risk.Models.Actions
{
    public class DeployAction : IAction
    {
        public IPlayer Player { get; set; }
        public Country CountryToDeploy { get; set; }
        public int NumberOfAddedTroops { get; set; }
        public bool ShowOnMap { get; set; }

        public DeployAction()
        {
            ShowOnMap = true;
        }

        public string GetMessage()
        {
            return string.Format("{0} added {1} troops in {2}", Player.Name, NumberOfAddedTroops,
                CountryToDeploy.GetName());
        }

        
    }
}