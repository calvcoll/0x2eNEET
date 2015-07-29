using System;
using System.Diagnostics;

namespace DotNEET.Extensions
{
    public static class BoolExts
    {
        public static bool ThrowIfFalse(this bool value)
        {
            return value.ThrowIfFalse(string.Empty);
        }

        public static bool ThrowIfFalse(this bool value, string message)
        {
            if (value)
            {
                return value;
            }
            throw new ArgumentException(message);
        }

        public static T ThrowIfFalse<T>(this T item, Func<T, bool> selector)
        {
            return item.ThrowIfFalse(selector, string.Empty);
        }

        public static T ThrowIfFalse<T>(this T item, Func<T, bool> selector, string message)
        {
            if (selector(item))
            {
                return item;
            }
            throw new ArgumentException(message);
        }

        public static bool ThrowIfTrue(this bool value)
        {
            return value.ThrowIfTrue(string.Empty);
        }

        public static bool ThrowIfTrue(this bool value, string message)
        {
            if (!value)
            {
                return value;
            }
            throw new ArgumentException(message);
        }

        public static T ThrowIfTrue<T>(this T item, Func<T, bool> selector)
        {
            return item.ThrowIfTrue(selector, string.Empty);
        }

        public static T ThrowIfTrue<T>(this T item, Func<T, bool> selector, string message)
        {
            if (!selector(item))
            {
                return item;
            }
            throw new ArgumentException(message);
        }
    }
}