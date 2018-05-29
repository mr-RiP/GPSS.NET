using GPSS.StandardAttributes;
using System;

namespace GPSS.Entities.Calculations
{
    internal class RandomGenerator : ICloneable, IRandomGeneratorAttributes
    {
        public int RandomNumber()
        {
            throw new NotImplementedException();
        }

        public RandomGenerator Clone()
        {
            throw new NotImplementedException();
        }

        object ICloneable.Clone() => Clone();
    }
}
