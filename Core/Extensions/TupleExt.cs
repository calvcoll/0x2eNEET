using System;

namespace DotNEET.Extensions
{
    public static class TupleExt
    {
        public static Tuple<TItemA, TItemB, TItemC> Append<TItemA, TItemB, TItemC>(this Tuple<TItemA, TItemB> source, TItemC itemToAdd)
        {
            return Tuple.Create(source.Item1, source.Item2, itemToAdd);
        }

        public static Tuple<TItemA, TItemB, TItemC, TItemD> Append<TItemA, TItemB, TItemC, TItemD>(this Tuple<TItemA, TItemB, TItemC> source, TItemD itemToAdd)
        {
            return Tuple.Create(source.Item1, source.Item2, source.Item3, itemToAdd);
        }

        public static Tuple<TItemA, TItemB, TItemC, TItemD, TItemE> Append<TItemA, TItemB, TItemC, TItemD, TItemE>(this Tuple<TItemA, TItemB, TItemC, TItemD> source, TItemE itemToAdd)
        {
            return Tuple.Create(source.Item1, source.Item2, source.Item3, source.Item4, itemToAdd);
        }
    }
}