using DotNEET.Functions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNEET.Extensions
{
    public static class IEnumerableExts
    {
        public static int ComposedHashCode<T>(this IEnumerable<T> objs)
        {
            return Hashs.ComposedHashCode(objs.Cast<object>().ToArray());
        }

        public static IEnumerable<T> IntersectMany<T>(this IEnumerable<IEnumerable<T>> src)
        {
            IEnumerable<T> ret = null;
            foreach (var subSrc in src)
            {
                if (ret == null)
                {
                    ret = subSrc;
                }
                else
                {
                    if (!ret.Any())
                    {
                        yield break;
                    }
                    ret = ret.Intersect(subSrc);
                }
            }
            foreach (var item in (ret ?? Enumerable.Empty<T>()))
            {
                yield return item;
            }
        }

        public static bool SequenceCover<TSource>(this IEnumerable<TSource> src, IEnumerable<TSource> other, Func<TSource, TSource, bool> equality)
        {
            src.ThrowIfNull();
            other.ThrowIfNull();
            using (var iterator = src.GetEnumerator())
            {
                foreach (var item in other)
                {
                    do
                    {
                        if (!iterator.MoveNext())
                        {
                            return false;
                        }
                    }
                    while (!equality(iterator.Current, item));
                }
            }
            return true;
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> src, int chunkSize)
        {
            while (src.Any())
            {
                yield return src.Take(chunkSize);
                src = src.Skip(chunkSize);
            }
        }

        public static HashSet<TReturn> ToHashSet<TSource, TReturn>(this IEnumerable<TSource> src, Func<TSource, TReturn> selector)
        {
            var ret = new HashSet<TReturn>();
            foreach (var item in src)
            {
                ret.Add(selector(item));
            }
            return ret;
        }

        public static IList<TSource> ToIList<TSource>(this IEnumerable<TSource> src)
        {
            return src.ToList();
        }
    }
}