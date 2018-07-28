using System;
using System.Collections.Generic;
using System.Linq;

namespace GPSS.Entities.Calculations.Functions
{
	internal class DiscreteAttributeFunction : Function
	{
		private DiscreteAttributeFunction()
		{
		}

		// http://www.minutemansoftware.com/reference/r6.htm#FUNCTION
		public DiscreteAttributeFunction(Func<IStandardAttributes, double> argument,
			IEnumerable<KeyValuePair<double, Func<IStandardAttributes, double>>> values)
		{
			Argument = argument;
			Values = values
				.Select(i => new KeyValuePair<double, Func<IStandardAttributes, double>>(i.Key, i.Value))
				.OrderBy(i => i.Key)
				.ToList();
		}

		public Func<IStandardAttributes, double> Argument { get; private set; }

		public List<KeyValuePair<double, Func<IStandardAttributes, double>>> Values { get; private set; }

		public override void Calculate(IStandardAttributes sna)
		{
			double argument = CalculateArgument(Argument, sna);
			var valueFunc = FindDiscreteValue(Values, argument);
			Result = CalculateAttributeResult(valueFunc, sna);
		}

		private double CalculateArgument(IStandardAttributes sna)
		{
			throw new NotImplementedException();
		}

		public override Function Clone() => new DiscreteAttributeFunction
		{
			Argument = Argument,
			Values = Values,
			Result = Result,
		};
	}
}
