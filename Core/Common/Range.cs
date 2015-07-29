using MoreLinq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DotNEET.Extensions;

namespace DotNEET
{
    public static class Range
    {
        public static bool BinarySearch<TKey, TValue>(this IReadOnlyList<Range<TKey, TValue>> ranges, TKey key, out TValue value) where TKey : IComparable<TKey>
        {
            int min = 0;
            int max = ranges.Count - 1;
            while (min <= max)
            {
                int mid = (min + max) / 2;
                int comparison = ranges[mid].CompareTo(key);
                if (comparison == 0)
                {
                    value = ranges[mid].Value;
                    return true;
                }
                if (comparison < 0)
                {
                    min = mid + 1;
                }
                else if (comparison > 0)
                {
                    max = mid - 1;
                }
            }
            value = default(TValue);
            return false;
        }

        public static Range<TKey, TValue> Create<TKey, TValue>(TKey bottom, TKey top, TValue value) where TKey : IComparable<TKey>
        {
            return new Range<TKey, TValue>(bottom, top, value);
        }

        /// <summary>
        /// Remove all overlapping ranges to keep only the smallest atomic ranges
        /// This mathematically isn't useful but it is when we need to get rid of some overlapping values in some cases
        /// </summary>
        /// <typeparam name="TKey">The Key Range type</typeparam>
        /// <typeparam name="TValue">The Value Range type</typeparam>
        /// <param name="source">Multiple ranges to merge</param>
        /// <returns>The distincts ranges without overlapping</returns>
        public static IEnumerable<Range<TKey, TValue>> ExceptOverlap<TKey, TValue>(this IEnumerable<Range<TKey, TValue>> source)
            where TKey : IComparable<TKey>
        {
            source = source.ToList(); // we assume it's a finite number of ranges since people are asking for it to be non overlapped
            var toReturn = source.ToHashSet();
            foreach (var item in source)
            {
                if (toReturn.Any(x => (item.Overlap(x) && (x != item))))
                {
                    toReturn.Remove(item);
                }
            }
            return toReturn;
        }

        /// <summary>
        /// Merge multiple range into a smaller or equal number of ranges
        /// Removing the overlapped ranges (values will be lost)
        /// Beside the values, nothing should be lost (ex :  [0, 2] & [1, 3] cannot be merged, but [0, 2] & [1, 2] yes)
        /// </summary>
        /// <typeparam name="TKey">The Key Range type</typeparam>
        /// <typeparam name="TValue">The Value Range type</typeparam>
        /// <param name="source">Multiple ranges to merge</param>
        /// <returns>The merged ranges</returns>
        public static IEnumerable<Range<TKey, TValue>> MergeUp<TKey, TValue>(this IEnumerable<Range<TKey, TValue>> source)
            where TKey : IComparable<TKey>
        {
            return source.Where(x => source.All(w => !(x.IsWithin(w) && (x != w)))); // this is valid since a range is within itself
        }

        public static Range<TKey, TValue> ToRange<TKey, TValue>(this TValue value, Func<TValue, TKey> bottomSelector, Func<TValue, TKey> topSelector)
            where TKey : IComparable<TKey>
        {
            return Range.Create(bottomSelector(value), topSelector(value), value);
        }

        public static IEnumerable<Range<TKey, TValue>> ToRanges<TKey, TValue>(this IEnumerable<TValue> source, Func<TValue, TKey> bottomSelector, Func<TValue, TKey> topSelector)
            where TKey : IComparable<TKey>
        {
            foreach (var value in source)
            {
                yield return value.ToRange(bottomSelector, topSelector);
            }
        }
    }

    [ImmutableObject(true)]
    public class Range<TKey, TValue> : IComparable<Range<TKey, TValue>>, IEquatable<Range<TKey, TValue>> where TKey : IComparable<TKey>
    {
        private readonly TKey bottom;
        private readonly TKey top;
        private readonly TValue value;

        public Range(TKey bottom, TKey top, TValue value)
        {
            if (bottom.CompareTo(top) > 0)
            {
                throw new ArgumentException("bot < top");
            }
            this.bottom = bottom;
            this.top = top;
            this.value = value;
        }

        public TKey Bottom
        {
            get
            {
                return this.bottom;
            }
        }

        public TKey Top
        {
            get
            {
                return this.bottom;
            }
        }

        public TValue Value
        {
            get
            {
                return this.value;
            }
        }

        public static bool operator !=(Range<TKey, TValue> first, Range<TKey, TValue> second)
        {
            return !(first == second);
        }

        public static bool operator ==(Range<TKey, TValue> first, Range<TKey, TValue> second)
        {
            if (object.ReferenceEquals(second, null))
            {
                if (object.ReferenceEquals(first, null))
                {
                    return true;
                }
                return false;
            }
            return first.Equals(second);
        }

        public int CompareTo(Range<TKey, TValue> other)
        {
            if ((bottom.CompareTo(other.bottom) < 0) && (top.CompareTo(other.top) < 0))
            {
                return -1;
            }
            if ((bottom.CompareTo(other.bottom) > 0) && (top.CompareTo(other.top) > 0))
            {
                return 1;
            }
            if ((bottom.CompareTo(other.bottom) == 0) && (top.CompareTo(other.top) == 0))
            {
                return 0;
            }
            throw new ArgumentException("Incomparable values (overlapping)");
        }

        /// <summary>
        /// Returns 0 if value is in the specified range;
        /// less than 0 if value is above the range;
        /// greater than 0 if value is below the range.
        /// </summary>
        public int CompareTo(TKey value)
        {
            if (value.CompareTo(this.bottom) < 0)
            {
                return 1;
            }
            if (value.CompareTo(this.top) > 0)
            {
                return -1;
            }
            return 0;
        }

        public bool Contains(TKey value)
        {
            return this.CompareTo(value) == 0;
        }

        public override bool Equals(object obj)
        {
            var casted = obj as Range<TKey, TValue>;
            if (this.ReferenceEquals(casted))
            {
                return true;
            }
            return this.Equals(casted);
        }

        public bool Equals(Range<TKey, TValue> other)
        {
            if (this.ReferenceEquals(other))
            {
                return true;
            }
            return this.ReferenceEquals(other) || (this.top.Equals(other.top) && this.bottom.Equals(other.bottom) && this.value.Equals(other.value));
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + this.bottom.GetHashCode();
            hash = hash * 23 + this.top.GetHashCode();
            hash = hash * 23 + this.value.GetHashCode();
            return hash;
        }

        public bool IsWithin(Range<TKey, TValue> other)
        {
            if (object.ReferenceEquals(this, other))
            {
                //Shortcut
                return true;
            }
            return (this.top.CompareTo(other.top) <= 0) && (this.bottom.CompareTo(other.bottom) >= 0);
        }

        // If this overlap other
        public bool Overlap(Range<TKey, TValue> other)
        {
            if (object.ReferenceEquals(this, other))
            {
                //Shortcut
                return true;
            }
            return (this.top.CompareTo(other.top) >= 0) && (this.bottom.CompareTo(other.bottom) <= 0);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if ((this.bottom.CompareTo(key) < 0) && (this.top.CompareTo(key) > 0))
            {
                value = this.value;
                return true;
            }
            value = default(TValue);
            return false;
        }
    }
}