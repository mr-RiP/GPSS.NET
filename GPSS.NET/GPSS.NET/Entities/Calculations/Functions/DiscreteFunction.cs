using GPSS.Enums;
using GPSS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPSS.Entities.Calculations.Functions
{
    internal class DiscreteFunction : Function
    {
        private DiscreteFunction()
        {
        }

        // http://www.minutemansoftware.com/reference/r6.htm#FUNCTION
        public DiscreteFunction(Func<IStandardAttributes, double> argument, IEnumerable<KeyValuePair<double, double>> values)
        {
            Argument = argument;
            Values = values.Select(i => new KeyValuePair<double, double>(i.Key, i.Value)).OrderBy(i => i.Key).ToList();
        }

        public Func<IStandardAttributes, double> Argument { get; private set; }

        public List<KeyValuePair<double, double>> Values { get; private set; }

        public override void Calculate(IStandardAttributes sna)
        {
            double argument = CalculateArgument(Argument, sna);
            Result = FindDiscreteValue(Values, argument);
        }

        public override Function Clone() => new DiscreteFunction
        {
            Argument = Argument,
            Values = Values,
            Result = Result,
        };
    }
}
