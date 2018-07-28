using System;
using System.Collections.Generic;
using System.Linq;

namespace GPSS.Entities.Calculations.Functions
{
	internal class ContinuousFunction : Function
	{
		private ContinuousFunction()
		{
		}

		// http://www.minutemansoftware.com/reference/r6.htm#FUNCTION
		public ContinuousFunction(Func<IStandardAttributes, double> argument, IEnumerable<KeyValuePair<double, double>> values)
		{
			Argument = argument;
			Values = values.Select(i => new KeyValuePair<double, double>(i.Key, i.Value)).OrderBy(i => i.Key).ToList();
		}

		public Func<IStandardAttributes, double> Argument { get; private set; }

		public List<KeyValuePair<double, double>> Values { get; private set; }

		public override void Calculate(IStandardAttributes sna)
		{
			double argument = CalculateArgument(Argument, sna);
			Result = LinearInterpolation(argument);
		}

		// https://en.wikipedia.org/wiki/Linear_interpolation
		private double LinearInterpolation(double argument)
		{
			if (argument <= Values.First().Key)
				return Values.First().Value;
			else if (argument >= Values.Last().Key)
				return Values.Last().Value;
			else
			{
				int highBoundIndex = Values.FindIndex(i => argument <= i.Key);

				var lowBound = Values[highBoundIndex - 1];
				var highBound = Values[highBoundIndex];

				return lowBound.Value +
					(argument - lowBound.Key) *
					(highBound.Value - lowBound.Value) / (highBound.Key - lowBound.Key);
			}
		}

		public override Function Clone() => new ContinuousFunction
		{
			Argument = Argument,
			Values = Values,
			Result = Result,
		};
	}
}
