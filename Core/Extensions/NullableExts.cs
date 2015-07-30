using System;

namespace DotNEET.Extensions
{
    public static class NullableExts
    {
        public static int NullableHashCode<T>(this T? nullable) where T : struct
        {
            return nullable.HasValue ? 0 : nullable.GetHashCode();
        }

        public static T ThrowIfNull<T>(this T? value) where T : struct
        {
            return value.ThrowIfNull(string.Empty);
        }

        public static T ThrowIfNull<T>(this T? value, string message) where T : struct
        {
            if (!value.HasValue)
            {
                throw new ArgumentNullException(message);
            }
            return value.Value;
        }
    }
}