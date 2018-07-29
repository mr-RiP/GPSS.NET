using System;
using System.Collections.Generic;
using System.Linq;

namespace GPSS.Entities.Calculations.Functions
{
    internal class DiscreteFunction : Function
    {
        private DiscreteFunction()
        {
        }

        // http://www.minutemansoftware.com/reference/r6.htm#FUNCTION
        public DiscreteFunction(Func<IStandardAttributes, double> argument, IDictionary<double, double> values)
        {
            Argument = argument;
            Values = new SortedList<double, double>(values);
        }

        public Func<IStandardAttributes, double> Argument { get; private set; }

        public SortedList<double, double> Values { get; private set; }

        public override void Calculate(IStandardAttributes sna)
        {
            double argument = CalculateArgument(Argument, sna);
            Result = FindDiscreteValue(Values.Select(v => v).ToList(), argument);
        }

        public override Function Clone() => new DiscreteFunction
        {
            Argument = Argument,
            Values = Values,
            Result = Result,
        };
    }
}
