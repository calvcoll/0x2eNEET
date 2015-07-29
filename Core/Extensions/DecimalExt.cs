namespace DotNEET.Extensions
{
    public static class DecimalExt
    {
        public static bool IsWithin(this decimal value, decimal minimum, decimal maximum)
        {
            return value >= minimum && value <= maximum;
        }

        public static bool IsWithin(this int value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }
    }
}