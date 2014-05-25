using System;
using System.Collections.Generic;
using Risk.Core;
using Risk.Models;

namespace Risk.Players.RS
{
    public class Continent
    {
        public EContinent Name { get; set; }
        public List<Country> MyCountries { get; set; }
        public List<Country> EnemyCountries { get; set; }
        public List<IPlayer> Opponents { get; set; }

        /// <summary>
        ///     Dominance is a value between 0.0 and 1.0 where
        ///     0.0 is no owned countries and 1.0 is all countries owned.
        /// </summary>
        public double Dominance
        {
            get
            {
                double total = MyCountries.Count + EnemyCountries.Count;
                // Exact comparisons of doubles can be tricky...
                if (Math.Abs(total) < double.Epsilon)
                    return 0;

                return MyCountries.Count/total;
            }
        }

        public bool IsOwned
        {
            get { return EnemyCountries.Count == 0 && MyCountries.Count > 0; }
        }
    }
}