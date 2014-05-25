using Risk.Core;
using Risk.Models;

namespace Risk.Players.RS
{
    public class Move
    {
        private readonly TurnManager turnManager;

        public Move(TurnManager turnManager)
        {
            this.turnManager = turnManager;
        }

        public Country FromCountry { get; set; }
        public Country ToCountry { get; set; }
        public int Troops { get; set; }

        public void Execute()
        {
            turnManager.MoveTroops(FromCountry, ToCountry, Troops);
        }
    }
}