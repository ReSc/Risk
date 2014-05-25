using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Risk.Core
{
    public class Statistics
    {
        public Dictionary<string, int> TimesWonByPlayer; 

        public Statistics()
        {
            TimesWonByPlayer = new Dictionary<string, int>();
        }

        public static Statistics Get()
        {
            if (HttpContext.Current.Application["stats"] == null)
            {
                HttpContext.Current.Application["stats"] = new Statistics();
            }

            return (Statistics)HttpContext.Current.Application["stats"];
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