using GPSS.StandardAttributes;
using System;

namespace GPSS.Entities.Calculations
{
    internal class Variable<T> : ICloneable, IVariableAttributes<T>
    {
        public T Result { get; set; }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
