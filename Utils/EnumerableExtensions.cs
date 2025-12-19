using System;
using System.Collections.Generic;

namespace RFRocketLibrary.Utils
{
    public static class EnumerableExtensions
    {
        #region Methods

        public static IEnumerable<T> GetAndRemoveAllFast<T>(this List<T> list, Predicate<T> match)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list), $"EnumerableExtensions.GetAndRemoveAllFast<T>() '{nameof(list)}' argument is null!");
            
            if (match == null)
                throw new ArgumentNullException(nameof(match), $"EnumerableExtensions.GetAndRemoveAllFast<T>() '{nameof(match)}' argument is null!");

            var removedList = new LinkedList<T>();
            for (var current = list.Count - 1; current >= 0; current--)
            {
                if (!match(list[current]))
                    continue;

                removedList.AddFirst(list[current]);
                
                var last = list.Count - 1;
                list[current] = list[last];
                list.RemoveAt(last);
            }

            return removedList;
        }

        public static int RemoveAllFast<T>(this List<T> list, Predicate<T> match)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list), $"EnumerableExtensions.RemoveAllFast<T>() '{nameof(list)}' argument is null!");
            
            if (match == null)
                throw new ArgumentNullException(nameof(match), $"EnumerableExtensions.RemoveAllFast<T>() '{nameof(match)}' argument is null!");

            var count = 0;
            for (var current = list.Count - 1; current >= 0; current--)
            {
                if (!match(list[current]))
                    continue;

                var last = list.Count - 1;
                list[current] = list[last];
                list.RemoveAt(last);
                count++;
            }

            return count;
        }

        public static void RemoveFast<T>(this List<T> list, int index)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list), $"EnumerableExtensions.RemoveFast<T>() '{nameof(list)}' argument is null!");
            
            var last = list.Count - 1;
            if (last < index)
                throw new ArgumentOutOfRangeException($"EnumerableExtensions.RemoveFast<T>() '{nameof(index)}' argument is out of range!");

            list[index] = list[last];
            list.RemoveAt(last);
        }

        #endregion
    }
}