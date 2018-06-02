using GPSS.StandardAttributes;
using System;

namespace GPSS.Entities.Calculations
{
    internal class Savevalue : ICloneable, ISavevalueAttributes
    {
        private Savevalue()
        {

        }

        public Savevalue(dynamic initialValue)
        {
            InitialValue = initialValue;
            Value = initialValue;
        }

        public dynamic InitialValue { get; set; }

        public dynamic Value { get; set; }

        public Savevalue Clone() => new Savevalue
        {
            InitialValue = InitialValue,
            Value = Value,
        };

        object ICloneable.Clone() => Clone();

        public void Clear()
        {

        }
    }
}
