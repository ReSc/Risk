using System.Collections.Generic;
using Risk.Players;
using Risk.Players.RS;

namespace Risk.Core
{
    public class Settings
    {
        public const int CountriesRequiredToWin = 25;
        public const int MaximumAmountOfTurns = 100;
        private static readonly List<IPlayer> players = new List<IPlayer>() { new Aad(), new BlitzKrieg(), new Pros(), new Remco() };

        public static List<IPlayer> Players
        {
            get { return players; }
        }
    }
}
