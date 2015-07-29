using System.Collections.Generic;
using System.Linq;

namespace DotNEET.Extensions
{
    public static class HashSetExts
    {
        public static bool ContainsAny<T>(this HashSet<T> container, IEnumerable<T> elements)
        {
            return elements.Any(element => container.Contains(element));
        }
    }
}