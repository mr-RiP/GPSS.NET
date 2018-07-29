using GPSS.StandardAttributes;
using System;

namespace GPSS.Entities.Calculations
{
    internal class RandomNumberGenerator : ICloneable, IRandomNumberGeneratorAttributes
    {
        private readonly object lockObject = new object();

        private readonly Random random;

        public RandomNumberGenerator()
        {
            random = new Random();
        }

        public RandomNumberGenerator(int seed)
        {
            random = new Random(seed);
        }

        public double RandomDouble()
        {
            lock (lockObject)
            {
                return random.NextDouble();
            }
        }

        public int RandomInteger()
        {
            lock (lockObject)
            {
                return random.Next(0, 1000);
            }
        }

        public RandomNumberGenerator Clone()
        {
            lock (lockObject)
            {
                return new RandomNumberGenerator(random.Next());
            }
        }

        object ICloneable.Clone() => Clone();
    }
}
