using System;
using System.Collections.Generic;
using System.Linq;

namespace SimAI.Core.Extensions {
    public static class ListExtensions {

        public static IList<T> InstantiateItems<T>(this IList<T> source, int count) where T : new() {

            if(count > 0) {
                for (int index = 0; index < count; index++) {
                    source.Add(new T());
                }
            }

            return source;
        }

        public static void ForEachReverse<T>(this IList<T> source, Action<T> action) {
            for (int count = source.Count - 1; count >= 0; count--) {
                action(source[count]);
            }
        }

        public static List<string> ToStringList<T>(this IEnumerable<T> input) {
            return input.Select(each => each.ToString()).ToList();
        } 

        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> fnRecurse) {
            foreach (T item in source) {
                yield return item;

                IEnumerable<T> seqRecurse = fnRecurse(item);
                if (seqRecurse != null) {
                    foreach (T itemRecurse in Traverse(seqRecurse, fnRecurse)) {
                        yield return itemRecurse;
                    }
                }
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this List<TSource> source, Func<TSource, TKey> keySelector) {

            var seenKeys = new HashSet<TKey>();

            foreach (TSource element in source) {
                if (seenKeys.Add(keySelector(element))) {
                    yield return element;
                }
            }
        }

        public static List<T> Shuffle<T>(this List<T> list) {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }

        public static int Find<T1, T2>(this List<T1> haystack, List<T2> needle, Func<T1, T2, bool> predicate) {

            if (!needle.Any() || needle.Count > haystack.Count)
                return -1;
            
            var items = haystack.ToList();

            while (items.Count >= needle.Count) {
                if (items.Take(needle.Count).ToList().Equals(needle, predicate))
                    return (haystack.Count - items.Count);

                items.RemoveAt(0);
            }
            

            return -1;
        }

        public static bool Equals<T1, T2>(this List<T1> list1, List<T2> list2, Func<T1, T2, bool> predicate) {
            if (list1.Count != list2.Count)
                return false;

            var index = 0;

            foreach (var item in list1) {
                if (!predicate(item, list2[index]))
                    return false;

                index += 1;
            }

            return true;
        }
    }
}