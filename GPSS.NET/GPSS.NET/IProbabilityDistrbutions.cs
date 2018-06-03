using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS
{
    public interface IProbabilityDistributions
    {
        /// <summary>
        /// Standard Uniform Distribution generator.
        /// </summary>
        /// <returns>
        /// Pseudo random double value between 0 and 1.
        /// </returns>
        double StandardUniform();

        /// <summary>
        /// Uniform Distribution generator.
        /// </summary>
        /// <param name="min">Result value lower bound.</param>
        /// <param name="max">Result value higher bound.</param>
        /// <returns>
        /// Pseudo random double value between <paramref name="min"/> and <paramref name="max"/> values.
        /// </returns>
        double Uniform(double min, double max);
    }
}
