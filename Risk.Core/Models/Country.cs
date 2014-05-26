using System.Collections.Generic;
using Risk.Core;

namespace Risk.Models
{
    public class Country
    {
        public Country()
        {
            NumberOfTroops = 1;
            AdjacentCountries = new List<Country>();
        }

        public int Number { get; set; }
        public EContinent Continent { get; set; }
        public int NumberOfTroops { get; set; }

        public int XPositionOnBoard { get; set; }
        public int YPositionOnBoard { get; set; }

        public List<Country> AdjacentCountries { get; set; }

        public IPlayer Owner { get; set; }

        public string GetName()
        {
            return string.Format("{0}-{1}", Continent, Number);
        }
    }
}