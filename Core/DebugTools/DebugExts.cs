using System;

namespace DotNEET.Debug
{
    public static class DebugExts
    {
        public static T Debug<T>(this T source, Predicate<T> breakIf, Action<T> breakFunc)
        {
#if DEBUG
            if (breakIf(source))
            {
                breakFunc(source);
            }
#endif
            return source;
        }
    }
}