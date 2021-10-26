using System;
using System.Collections.Generic;

namespace GBX.NET
{
    public static class ListExtensions
    {
        /// <summary>
        /// Removes all elements from the list based on a match.
        /// </summary>
        /// <typeparam name="T">List type.</typeparam>
        /// <param name="list">List.</param>
        /// <param name="match">Match.</param>
        /// <returns>Amount of removed objects.</returns>
        /// <exception cref="NotSupportedException">The list is read-only.</exception>
        public static int RemoveAll<T>(this IList<T> list, Predicate<T> match)
        {
            int count = 0;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (match(list[i]))
                {
                    ++count;
                    list.RemoveAt(i);
                }
            }

            return count;
        }
    }
}
