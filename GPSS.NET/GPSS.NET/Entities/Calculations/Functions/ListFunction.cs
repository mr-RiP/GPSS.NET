using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.Calculations.Functions
{
    internal class ListFunction : Function
    {
        private ListFunction()
        {
        }

        // http://www.minutemansoftware.com/reference/r6.htm#FUNCTION
        public ListFunction(Func<IStandardAttributes, int> argument, IList<double> values)
        {
            Argument = argument;
            Values = new List<double>(values);
        }

        public Func<IStandardAttributes, int> Argument { get; private set; }
        public List<double> Values { get; private set; }

        public override void Calculate(IStandardAttributes sna)
        {
            int argument = CalculateArgument(Argument, sna);
            Result = GetListValue(Values, argument);
        }

        public override Function Clone() => new ListFunction
        {
            Argument = Argument,
            Values = Values,
            Result = Result,
        };
    }
}
