using GPSS.StandardAttributes;
using System;

namespace GPSS.Entities.Groups
{
    internal class NumericGroup : ICloneable, INumericGroupAttributes
    {
        public int Count => throw new NotImplementedException();

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
