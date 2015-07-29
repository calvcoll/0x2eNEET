using System;

namespace DotNEET.Extensions
{
    public static class TypeExts
    {
        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }
    }
}