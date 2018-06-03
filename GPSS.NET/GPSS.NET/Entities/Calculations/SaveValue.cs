using GPSS.Extensions;
using GPSS.StandardAttributes;
using System;

namespace GPSS.Entities.Calculations
{
    internal class Savevalue : ICloneable, ISavevalueAttributes
    {
        public dynamic Value { get; set; }

        public Savevalue Clone() => new Savevalue
        {
            Value = Value,
        };

        object ICloneable.Clone() => Clone();
    }
}
