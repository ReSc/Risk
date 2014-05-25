using Risk.Core;
using Risk.Models;

namespace Risk.Players.RS
{
    public class Attack
    {
        private readonly TurnManager _turnManager;

        public Attack(TurnManager turnManager)
        {
            _turnManager = turnManager;
        }

        public void Execute()
        {
            Succeeded = _turnManager.Attack(FromCountry, ToCountry, Troops);
        }

        public Country FromCountry { get; set; }
        public Country ToCountry { get; set; }
        public int Troops { get; set; }
        public bool? Succeeded { get; set; }
    }
}