using System;

namespace DotNEET.Extensions
{
    public static class TypeExts
    {
        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static T GetInstanceOrNull<T>(this Type type) 
            where T : class
        {
            return type.GetInstanceOrNull() as T;
        }

        public static object GetInstanceOrNull(this Type type)
        {
            var contructor = type.GetConstructor(Type.EmptyTypes);
            return contructor != null ? contructor.Invoke(new object[] { }) : null;
        }

        public static bool HasEmptyContructor(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes) != null;
        }
    }
}