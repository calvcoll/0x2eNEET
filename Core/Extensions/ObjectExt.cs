using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace DotNEET.Extensions
{
    public static class ObjectExt
    {
        public static string ClassName<TSource>(this TSource source)
        {
            return typeof(TSource).Name;
        }

        public static string ConvertToString(this object item)
        {
            return System.Convert.ToString(item);
        }

        public static string PropertyName<TSource, TProp>(this TSource source, Expression<Func<TSource, TProp>> expression)
        {
            return ((MemberExpression)expression.Body).Member.Name;
        }

        [DebuggerStepThrough]
        public static bool ReferenceEquals<T, U>(this T first, U second)
            where T : class
            where U : class
        {
            return object.ReferenceEquals(first, second);
        }

        [DebuggerStepThrough]
        public static T ThrowIfNull<T>(this T value, string variableName)
        {
            if (value == null)
            {
                throw new NullReferenceException(string.Format("Value is Null: {0}", variableName));
            }

            return value;
        }

        [DebuggerStepThrough]
        public static T ThrowIfNull<T>(this T value)
        {
            return value.ThrowIfNull(string.Empty);
        }
    }
}