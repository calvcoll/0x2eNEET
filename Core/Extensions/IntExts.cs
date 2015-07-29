using System;

namespace DotNEET.Extensions
{
    public static class IntExts
    {
        public static int Modulo(this int value, int mod)
        {
            if (mod <= 0)
            {
                throw new ArgumentException("negative or null modulo are not handled !");
            }
            if (value < 0)
            {
                return mod - (Math.Abs(value) % mod);
            }
            return value % mod;
        }
    }
}