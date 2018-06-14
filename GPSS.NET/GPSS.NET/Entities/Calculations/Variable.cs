using GPSS.Enums;
using GPSS.Exceptions;
using GPSS.Extensions;
using GPSS.StandardAttributes;
using System;

namespace GPSS.Entities.Calculations
{
    internal class Variable<T> : ICloneable, IVariableAttributes<T>, ICalculatable<T>
    {
        public Variable(Func<IStandardAttributes, T> expression)
        {
            Expression = expression;
        }

        public Func<IStandardAttributes, T> Expression { get; set; }

        public T Result { get; private set; }

        public void Calculate(IStandardAttributes sna)
        {
            try
            {
                Result = Expression(sna);
            }
            catch (StandardAttributeAccessException error)
            {
                var entityType = GetEntityType();
                throw new StandardAttributeAccessException(
                    entityType.ToString() + " Expression could not access " + error.EntityType.ToString() + " attributes.",
                    entityType,
                    error);
            }
            catch (Exception error)
            {
                var entityType = GetEntityType();
                throw new StandardAttributeAccessException(
                    entityType.ToString() + " Expression could not been calculated.",
                    entityType,
                    error);
            }
        }

        public Variable<T> Clone() => new Variable<T>(Expression);      
        object ICloneable.Clone() => Clone();

        private EntityType GetEntityType()
        {
            if (Result is double)
                return EntityType.FloatVariable;
            else if (Result is bool)
                return EntityType.BoolVariable;
            else
                return EntityType.Variable;
        }
    }
}
