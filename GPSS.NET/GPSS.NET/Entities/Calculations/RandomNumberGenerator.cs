using GPSS;
using GPSS.StandardAttributes;
using System;

namespace GPSS.Entities.Calculations
{
    internal class RandomNumberGenerator : ICloneable, IRandomNumberGeneratorAttributes, IProbabilityDistributions
    {
        // 0 < x < 1
        // 0.99999999999999978 - верхний предел метода Random.NextDouble()
        // https://msdn.microsoft.com/ru-ru/library/system.random.nextdouble(v=vs.110).aspx
        // 4.94065645841247E-324 = (1.0 - 0.99999999999999978) / 2.0
        private const double nextDoubleFix = 4.94065645841247E-324;

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

        public IProbabilityDistributions ProbabilityDistributions => this;

        public double StandardUniform()
        {
            lock (lockObject)
            {              
                return random.NextDouble() + nextDoubleFix;
            }
        }

        public double Uniform(double min, double max)
        {
            return StandardUniform() * (max - min) + min;
        }

        public int RandomNumber()
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
