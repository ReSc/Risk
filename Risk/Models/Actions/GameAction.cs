using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Risk.Core;

namespace Risk.Models.Actions
{
    public class GameAction : IAction
    {
        public IPlayer Player { get; set; }
        public bool ShowOnMap { get; set; }
        public EPhase Phase { get; set; }
        public int TroopsToDeploy { get; set; }
        public int Turn { get; set; }

        public string GetMessage()
        {
            string suffix = string.Empty;

            if (Phase == EPhase.Deploy)
            {
                suffix = string.Format("-- {0} troops -----------------", TroopsToDeploy);
            }

            return string.Format("-----{0}----- {1} phase ---- {2} ---- {3}", Turn, Phase, Player.Name, suffix);
        }
    }
}