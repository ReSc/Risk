using System.Collections.Generic;
using System.Linq;
using Risk.Core;
using Risk.Models;

namespace Risk.Players.RS
{
    public static class _GameInformationExtensions
    {
        public static IEnumerable<EContinent> GetContinentsOwnedBy(this GameInformation info, IPlayer player)
        {
            return from c in info.GetAllCountries()
                   group c by c.Continent
                   into grp
                   where grp.All(x => x.Owner == player)
                   select grp.Key;
        }

        public static bool IsContinentGateway(this Country c)
        {
            return c.AdjacentCountries.Any(x => x.Continent != c.Continent);
        }

        public static bool IsContinentGatewayUnderAttack(this Country c)
        {
            return c.IsContinentGateway() && c.IsUnderAttack();
        }

        public static bool IsUnderAttack(this Country c)
        {
            return c.GetAttackingCountries().Any();
        }

        public static IEnumerable<Country> GetAttackingCountries(this Country c)
        {
            return c.AdjacentCountries.Where(x => x.Owner != c.Owner);
        }

        public static int GetThreatLevel(this Country c)
        {
            return c.GetAttackingCountries().Max(x => x.NumberOfTroops) - c.NumberOfTroops;
        }

        public static IEnumerable<Country> GetAttackingCountriesByDescendingThreatLevel(this Country c)
        {
            return c.GetAttackingCountries().OrderBy(x => x.NumberOfTroops);
        }
    }
}