using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Extensions
{
    public static class ProbabilityDistributions
    {
        // 0 < x < 1
        // 0.99999999999999978 - верхний предел метода Random.NextDouble()
        // https://msdn.microsoft.com/ru-ru/library/system.random.nextdouble(v=vs.110).aspx
        // 4.94065645841247E-324 = (1.0 - 0.99999999999999978) / 2.0
        private const double randomDoubleFix = 4.94065645841247E-324;

        /// <summary>
        /// Standard Uniform Distribution generator.
        /// </summary>
        /// <returns>
        /// Pseudo random double value between 0 and 1.
        /// </returns>
        public static double StandardUniform(this IRandomNumberGeneratorAttributes generator)
        {
            return generator.RandomDouble() - randomDoubleFix;
        }

        /// <summary>
        /// Uniform Distribution generator.
        /// </summary>
        /// <param name="min">Result value lower bound.</param>
        /// <param name="max">Result value higher bound.</param>
        /// <returns>
        /// Pseudo random double value between <paramref name="min"/> and <paramref name="max"/> values.
        /// </returns>
        public static double Uniform(this IRandomNumberGeneratorAttributes generator, double min, double max)
        {
            return generator.StandardUniform() * (max - min) + min;
        }

        /// <summary>
        /// Standard Normal Distribution generator.
        /// </summary>
        /// <returns>
        /// Pseudo random double value between 0 and 1.
        /// </returns>
        public static double StandardNormal(this IRandomNumberGeneratorAttributes generator)
        {
            double sum = 0.0;
            for (int i = 0; i < 12; ++i)
                sum += generator.StandardUniform();
            sum -= 6.0;
            sum /= 6.0;
            return sum;
        }

        /// <summary>
        /// Normal Distribution generator.
        /// </summary>
        /// <param name="mean">Result value mean.</param>
        /// <param name="standardDeviation">Result value Standard Deviation.</param>
        /// <returns>Pseudo random normal-distributed value.</returns>
        public static double Normal(this IRandomNumberGeneratorAttributes generator, 
            double mean, double standardDeviation)
        {
            return generator.StandardNormal() * standardDeviation + mean;
        }


    }
}
