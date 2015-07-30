using System;

namespace DotNEET.Extensions
{
    public static class StringExts
    {
        public static T ConvertTo<T>(this string value)
        {
            return (T)value.ConvertTo(typeof(T));
        }

        public static object ConvertTo(this string value, Type changeType)
        {
            if (changeType == typeof(Guid))
            {
                return Guid.Parse(value);
            }
            if (string.IsNullOrEmpty(value) && changeType.IsNullable())
            {
                return Activator.CreateInstance(changeType);
            }
            else
            {
                return Convert.ChangeType(value, Nullable.GetUnderlyingType(changeType) ?? changeType);
            }
        }

        public static string ThrowIfNullOrEmpty(this string str, string description = null)
        {
            return str.ThrowIfTrue(string.IsNullOrEmpty, "The string is not null or white space : " + (description ?? string.Empty));
        }

        public static string ThrowIfNullOrWhiteSpace(this string str, string description = null)
        {
            return str.ThrowIfTrue(string.IsNullOrWhiteSpace, "The string is not null or white space : " + (description ?? string.Empty));
        }
    }
}