using GPSS.StandardAttributes;
using System;

namespace GPSS.Entities.Calculations
{
    internal class Savevalue : ICloneable, ISavevalueAttributes
    {
        public dynamic Value { get; set; }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
