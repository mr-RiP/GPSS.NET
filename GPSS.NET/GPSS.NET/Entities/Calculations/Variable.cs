using GPSS.Extensions;
using GPSS.StandardAttributes;
using System;

namespace GPSS.Entities.Calculations
{
    internal class Variable<T> : ICloneable, IVariableAttributes<T>
    {     
        public T Result { get; set; }

        public Variable<T> Clone() => new Variable<T>
        {
            Result = Result
        };
        
        object ICloneable.Clone() => Clone();
    }
}
