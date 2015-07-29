using System;
using System.Windows;

namespace DotNEET.Functions
{
    public static class Length
    {
        public static double Between(double xA, double yA, double xB, double yB)
        {
            return Math.Sqrt(Math.Pow(xB - xA, 2) + Math.Pow(yB - yA, 2));
        }

        public static double Between(Tuple<double, double> a, Tuple<double, double> b)
        {
            return Length.Between(a.Item1, a.Item2, b.Item1, b.Item2);
        }
    }
}