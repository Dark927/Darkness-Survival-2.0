using System;
using System.Collections.Generic;
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


        /// <summary>
        /// This method shuffles the first N elements of the list within a specified range.
        /// It ensures that only the first `firstElementsToShuffle` elements are shuffled, and the shuffle happens within the specified `shuffleRangeLimit`.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list of elements to shuffle.</param>
        /// <param name="firstElementsToShuffle">The number of elements at the start of the list to shuffle.</param>
        /// <param name="shuffleRangeLimit">The maximum range within which elements can be shuffled. The shuffle will only occur within this range starting from the first element.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when either the `firstElementsToShuffle` or `shuffleRangeLimit` exceeds the size of the list.
        /// </exception>
        public static void ShuffleElementsWithRange<T>(this List<T> list, int firstElementsToShuffle, int shuffleRangeLimit)
        {
            int elementsCount = list.Count;

            // Validate inputs
            if (firstElementsToShuffle > elementsCount || (shuffleRangeLimit > elementsCount))
            {
                throw new ArgumentException("The number of elements to shuffle or the shuffle range exceeds the total number of elements in the list.");
            }

            int randomIndex = 0;

            for (int i = 0; i < firstElementsToShuffle; ++i)
            {
                randomIndex = UnityEngine.Random.Range(i, shuffleRangeLimit);

                // Swap the elements
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }
    }
}
