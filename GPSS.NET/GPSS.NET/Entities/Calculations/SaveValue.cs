using GPSS.StandardAttributes;
using System;

namespace GPSS.Entities.Calculations
{
    internal class Savevalue : ICloneable, ISavevalueAttributes
    {
        public dynamic Value { get; set; }

        public Savevalue Clone()
        {
            throw new NotImplementedException();
        }

        object ICloneable.Clone() => Clone();
    }
}
