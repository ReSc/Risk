using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Risk.Core;

namespace Risk.Models.Actions
{
    public class MoveAction : IAction
    {
        public IPlayer Player { get; set; }
        public bool ShowOnMap { get; set; }

        public Country Country { get; set; }
        public Country CountryToMoveTo { get; set; }

        public int NumberOfTroopsToMove { get; set; }

        public MoveAction()
        {
            ShowOnMap = true;
        }

        public string GetMessage()
        {
            return string.Format("{0} moved {1} troops from {2} to {3}", 
                Player.Name, NumberOfTroopsToMove, Country.GetName(), CountryToMoveTo.GetName());
        }
    }
}