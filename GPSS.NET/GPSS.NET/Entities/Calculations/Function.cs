using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.Calculations
{
    internal class Function : ICloneable, IFunctionAttributes
    {
        public double Result => throw new NotImplementedException();

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
