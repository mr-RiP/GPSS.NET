using GPSS.Extensions;
using GPSS.StandardAttributes;
using System;

namespace GPSS.Entities.Calculations
{
    internal class Variable<T> : ICloneable, IVariableAttributes<T>
    {
        public Variable(Func<IStandardAttributes, T> expression)
        {
            Expression = expression;
        }

        public Func<IStandardAttributes, T> Expression { get; set; }

        public T Result { get; private set; }

        public void Calculate(IStandardAttributes sna)
        {
            Result = Expression(sna);
        }

        public Variable<T> Clone() => new Variable<T>(Expression);      
        object ICloneable.Clone() => Clone();
    }
}
