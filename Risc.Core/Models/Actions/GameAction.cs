using Risk.Core;

namespace Risk.Models.Actions
{
    public class GameAction : IAction
    {
        public EPhase Phase { get; set; }
        public int TroopsToDeploy { get; set; }
        public int Turn { get; set; }
        public IPlayer Player { get; set; }
        public bool ShowOnMap { get; set; }

        public string GetMessage()
        {
            var suffix = string.Empty;

            if (Phase == EPhase.Deploy)
            {
                suffix = string.Format("-- {0} troops -----------------", TroopsToDeploy);
            }

            return string.Format("-----{0}----- {1} phase ---- {2} ---- {3}", Turn, Phase, Player.Name, suffix);
        }
    }
}