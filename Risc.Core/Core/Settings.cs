using System.Collections.Generic;
using System.Linq;
using Risk.Players;
using Risk.Players.RS;

namespace Risk.Core
{
    public class Settings
    {
        public int CountriesRequiredToWin { get; set; }
        public int MaximumAmountOfTurns { get; set; }
        public List<IPlayer> Players { get; set; }

        public Settings() : this(new IPlayer[] { new Aad(), new BlitzKrieg(), new Pros(), new Remco(), }, 25, 100) { }

        public Settings(IEnumerable<IPlayer> players, int countriesRequiredToWin, int maximumAmountOfTurns)
        {
            Players = players.ToList();
            CountriesRequiredToWin = countriesRequiredToWin;
            MaximumAmountOfTurns = maximumAmountOfTurns;
        }
    }
}