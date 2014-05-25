using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Risk.Models;

namespace Risk.Core
{
    public interface IPlayer
    {
        string Name { get; }
        string Color { get; }

        void Deploy(int numberOfTroops);
        void Attack();
        void Move();

        int Defend(List<int> attackRolls, Country countryToDefend);
    }
}
