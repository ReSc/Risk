using System.Collections.Generic;
using Risk.Models;

namespace Risk.Core
{
    public interface IPlayer
    {
        string Name { get; }
        string Color { get; }

        void Deploy(TurnManager turnManager, int numberOfTroops);
        void Attack(TurnManager turnManager);
        void Move(TurnManager turnManager);

        int Defend(TurnManager turnManager, List<int> attackRolls, Country countryToDefend);
    }
}