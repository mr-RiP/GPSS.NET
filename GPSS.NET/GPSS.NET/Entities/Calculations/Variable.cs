using GPSS.StandardAttributes;
using System;

namespace GPSS.Entities.Calculations
{
    internal class Variable<T> : ICloneable, IVariableAttributes<T>
    {
        private Variable()
        {

        }

        public Variable(T initialValue)
        {
            InitialValue = initialValue;
            Result = initialValue;
        }
     
        public T InitialValue { get; private set; }

        public T Result { get; set; }

        public Variable<T> Clone() => new Variable<T>
        {
            InitialValue = InitialValue,
            Result = Result
        };
        
        object ICloneable.Clone() => Clone();

        public void Clear()
        {
            Result = InitialValue;
        }
    }
}
