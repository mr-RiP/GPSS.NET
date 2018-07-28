using System;
using System.Collections.Generic;

namespace GPSS.Entities.Calculations.Functions
{
	internal class AttributeListFunction : Function
	{
		private AttributeListFunction()
		{
		}

		// http://www.minutemansoftware.com/reference/r6.htm#FUNCTION
		public AttributeListFunction(Func<IStandardAttributes, int> argument, IList<Func<IStandardAttributes, double>> values)
		{
			Argument = argument;
			Values = new List<Func<IStandardAttributes, double>>(values);
		}

		public Func<IStandardAttributes, int> Argument { get; private set; }
		public List<Func<IStandardAttributes, double>> Values { get; private set; }

		public override void Calculate(IStandardAttributes sna)
		{
			int argument = CalculateArgument(Argument, sna);
			var valueFunc = GetListValue(Values, argument);
			Result = CalculateAttributeResult(valueFunc, sna);
		}

		public override Function Clone() => new AttributeListFunction
		{
			Argument = Argument,
			Values = Values,
			Result = Result,
		};
	}
}
