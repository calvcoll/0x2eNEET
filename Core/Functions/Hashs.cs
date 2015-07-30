namespace DotNEET.Functions
{
    public static class Hashs
    {
        public static int ComposedHashCode(params object[] objs)
        {
            int hash = 17;
            foreach (var obj in objs)
            {
                hash = hash * 23 + obj.GetHashCode();
            }
            return hash;
        }
    }
}