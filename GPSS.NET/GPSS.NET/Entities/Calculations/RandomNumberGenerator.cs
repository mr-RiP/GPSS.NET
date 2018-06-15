using GPSS;
using GPSS.StandardAttributes;
using System;

namespace GPSS.Entities.Calculations
{
    internal class RandomNumberGenerator : ICloneable, IRandomNumberGeneratorAttributes
    {
        public RandomNumberGenerator()
        {
            random = new Random();
        }

        public RandomNumberGenerator(int seed)
        {
            random = new Random(seed);
        }

        private RandomNumberGenerator(object lockObject, Random random)
        {
            this.lockObject = lockObject;
            this.random = random;
        }

        private object lockObject = new object();
        private Random random;

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

        public RandomNumberGenerator Clone() => new RandomNumberGenerator(lockObject, random);
        object ICloneable.Clone() => Clone();
    }
}
