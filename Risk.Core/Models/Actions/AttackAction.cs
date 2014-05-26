using Risk.Core;

namespace Risk.Models.Actions
{
    public class AttackAction : IAction
    {
        public AttackAction()
        {
            ShowOnMap = true;
        }

        public Country Country { get; set; }
        public Country CountryToAttack { get; set; }

        public IPlayer DefendingPlayer { get; set; }

        public int Attackers { get; set; }
        public int Defenders { get; set; }

        public int DeadAttackers { get; set; }
        public int DeadDefenders { get; set; }

        public bool AttackSucceeded { get; set; }
        public IPlayer Player { get; set; }
        public bool ShowOnMap { get; set; }

        public string GetMessage()
        {
            var part1 = string.Format("{0} ({1}) attacks {2} ({3}) {4} vs {5}",
                                      Country.GetName(), Player.Name, CountryToAttack.GetName(),
                                      DefendingPlayer.Name, Attackers, Defenders);
            var part2 = AttackSucceeded
                            ? string.Format("attackers won, {0} attackers died", DeadAttackers)
                            : string.Format("defenders won, {0} defenders died", DeadDefenders);

            return string.Format("{0} {1}", part1, part2);
        }
    }
}