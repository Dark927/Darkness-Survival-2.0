using System.Collections.Generic;
using System;
using System.Linq;

namespace Utilities
{
    public static class ListExtensions
    {
        /// <summary>
        /// Moves elements that match the condition from the source list to the destination list.
        /// </summary>
        public static void MoveRange<T>(this List<T> src, List<T> dest, Func<T, bool> predicate)
        {
            var itemsToMove = src.Where(predicate).ToList();

            if (itemsToMove.Count == 0)
            {
                return; // Nothing to move
            }

            dest.AddRange(itemsToMove);
            src.RemoveAll(itemsToMove.Contains);
        }
    }
}