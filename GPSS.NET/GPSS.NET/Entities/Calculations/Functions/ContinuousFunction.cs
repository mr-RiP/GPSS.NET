using System;
using System.Collections.Generic;

namespace GPSS.Entities.Calculations.Functions
{
	internal class ContinuousFunction : Function
	{
		private ContinuousFunction()
		{
		}

		// http://www.minutemansoftware.com/reference/r6.htm#FUNCTION
		public ContinuousFunction(Func<IStandardAttributes, double> argument, IDictionary<double, double> values)
		{
			Argument = argument;
			Values = new SortedList<double, double>(values);
		}

		public Func<IStandardAttributes, double> Argument { get; private set; }

		public SortedList<double, double> Values { get; private set; }

		public override void Calculate(IStandardAttributes sna)
		{
			double argument = CalculateArgument(Argument, sna);
			Result = LinearInterpolation(argument);
		}

		// https://en.wikipedia.org/wiki/Linear_interpolation
		private double LinearInterpolation(double argument)
		{
			var keys = Values.Keys;
			double minKey = keys[0];
			double maxKey = keys[keys.Count - 1];

			if (argument <= minKey)
			{
				return Values[minKey];
			}
			else if (argument >= maxKey)
			{
				return Values[maxKey];
			}
			else
			{
				int index = BinarySearchIndex(argument);

				double highBound = keys[index];
				double lowBound = keys[index - 1];

				return Values[lowBound] +
					(argument - lowBound) *
					(Values[highBound] - Values[lowBound]) / (highBound - lowBound);
			}
		}

		public override Function Clone() => new ContinuousFunction
		{
			Argument = Argument,
			Values = Values,
			Result = Result,
		};

		private int BinarySearchIndex(double argument)
		{
			var keys = Values.Keys;
			int minIndex = 0;
			int maxIndex = keys.Count - 1;

			while (minIndex <= maxIndex)
			{
				int currentIndex = (maxIndex - minIndex) / 2;

				int comparisonResult = CompareIndex(currentIndex, argument);
				if (comparisonResult > 0)
					minIndex = currentIndex + 1;
				else if (comparisonResult < 0)
					maxIndex = currentIndex - 1;
				else
					return currentIndex;
			}

			return minIndex;
		}

		private int CompareIndex(int index, double argument)
		{
			if (argument > Values.Keys[index])
			{
				return 1;
			}
			else if (argument < Values.Keys[index - 1])
			{
				return -1;
			}
			else
			{
				return 0;
			}
		}

	}
}
