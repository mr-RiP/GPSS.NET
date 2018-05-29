using GPSS.StandardAttributes;
using System;

namespace GPSS.Entities.Groups
{
    internal class NumericGroup : ICloneable, INumericGroupAttributes
    {
        public int Count => throw new NotImplementedException();

        public NumericGroup Clone()
        {
            throw new NotImplementedException();
        }

        object ICloneable.Clone() => Clone();
    }
}
