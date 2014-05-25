using System;
using System.Collections.Generic;
using System.Linq;

namespace Risk.Players.RS
{
    /// <summary> some usefull collection extensions </summary>
    public static class _CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
                collection.Add(item);
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items)
        {
            var set = new HashSet<T>();
            foreach (var item in items)
                set.Add(item);
            return set;
        }

        public static IEnumerable<TResult> Distinct<T, TResult>(this IEnumerable<T> items, Func<T, TResult> selector)
        {
            return items.Select(selector).Distinct();
        }
    }
}