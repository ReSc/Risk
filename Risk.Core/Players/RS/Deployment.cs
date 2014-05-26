using Risk.Core;
using Risk.Models;

namespace Risk.Players.RS
{
    public class Deployment
    {
        private readonly TurnManager turnManager;

        public Deployment(TurnManager turnManager)
        {
            this.turnManager = turnManager;
        }

        public Country ToCountry { get; set; }
        public int Troops { get; set; }

        public void Execute()
        {
            turnManager.DeployTroops(ToCountry, Troops);
        }
    }
}