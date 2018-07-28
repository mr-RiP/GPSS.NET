using GPSS.Enums;
using GPSS.Exceptions;
using GPSS.Extensions;
using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPSS.Entities.Calculations
{
	internal abstract class Function : ICloneable, IFunctionAttributes, ICalculatable<double>
	{
		public double Result { get; protected set; }

		public abstract void Calculate(IStandardAttributes sna);

		public abstract Function Clone();

		object ICloneable.Clone() => Clone();

		protected static T CalculateArgument<T>(Func<IStandardAttributes, T> argumentFunction, IStandardAttributes sna)
		{
			try
			{
				return argumentFunction(sna);
			}
			catch (Exception error)
			{
				throw new StandardAttributeAccessException(
					"Function argument value could not been calculated.",
					EntityType.Function,
					error);
			}
		}

		protected static T FindDiscreteValue<T>(IList<KeyValuePair<double, T>> values, double argument)
		{
			if (argument > values.Last().Key)
				return values.Last().Value;
			else
				return values.FirstOrDefault(kvp => kvp.Key >= argument).Value;
		}

		protected static T GetListValue<T>(List<T> values, int argument)
		{
			try
			{
				return values[argument];
			}
			catch (Exception error)
			{
				throw new StandardAttributeAccessException(
					"Function could not access list value with at given argument index.",
					EntityType.Function,
					error);
			}
		}

		protected static double CalculateAttributeResult(Func<IStandardAttributes, double> valueFunc, IStandardAttributes sna)
		{
			try
			{
				return valueFunc(sna);
			}
			catch (Exception error)
			{
				throw new StandardAttributeAccessException(
					"Function Result could not been calculated.",
					EntityType.Function,
					error);
			}
		}
	}
}
