using System.Collections.Generic;

namespace Risk.Core
{
    public class Statistics
    {
        public Dictionary<string, int> TimesWonByPlayer;

        public Statistics()
        {
            TimesWonByPlayer = new Dictionary<string, int>();
        }

        public void AddCurrentGameResults(IPlayer winner)
        {
            if (!TimesWonByPlayer.ContainsKey(winner.Name))
            {
                TimesWonByPlayer.Add(winner.Name, 1);
            }
            else
            {
                TimesWonByPlayer[winner.Name]++;
            }
        }
    }
}