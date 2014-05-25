using System.Collections.Generic;
using Risk.Models;

namespace Risk.Core
{
    public interface IPlayer
    {
        string Name { get; }
        string Color { get; }

        void Deploy(GameManager gameManager, int numberOfTroops);
        void Attack(GameManager gameManager);
        void Move(GameManager gameManager);

        int Defend(GameManager gameManager, List<int> attackRolls, Country countryToDefend);
    }
}