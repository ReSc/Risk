using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Risk.Core;

namespace Risk.Models.Actions
{
    public class AttackAction : IAction
    {
        public IPlayer Player { get; set; }
        public bool ShowOnMap { get; set; }

        public Country Country { get; set; }
        public Country CountryToAttack { get; set; }

        public IPlayer DefendingPlayer { get; set; }

        public int Attackers { get; set; }
        public int Defenders { get; set; }

        public int DeadAttackers { get; set; }
        public int DeadDefenders { get; set; }

        public bool AttackSucceeded { get; set; }

        public AttackAction()
        {
            ShowOnMap = true;
        }

        public string GetMessage()
        {
            string part1 = string.Format("{0} ({1}) attacks {2} ({3}) {4} vs {5}", 
                                            Country.GetName(), Player.Name, CountryToAttack.GetName(), 
                                            DefendingPlayer.Name, Attackers, Defenders);
            string part2 = AttackSucceeded
                ? string.Format("attackers won, {0} attackers died", DeadAttackers)
                : string.Format("defenders won, {0} defenders died", DeadDefenders);

            return string.Format("{0} {1}", part1, part2);
        }
    }
}