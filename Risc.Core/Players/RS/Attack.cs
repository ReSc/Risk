using Risk.Core;
using Risk.Models;

namespace Risk.Players.RS
{
    public class Attack
    {
        private readonly TurnManager turnManager;

        public Attack(TurnManager turnManager)
        {
            this.turnManager = turnManager;
        }

        public Country FromCountry { get; set; }
        public Country ToCountry { get; set; }
        public int Troops { get; set; }
        public bool? Succeeded { get; set; }

        public void Execute()
        {
            Succeeded = turnManager.Attack(FromCountry, ToCountry, Troops);
        }
    }
}