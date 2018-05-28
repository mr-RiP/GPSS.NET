using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.Calculations
{
    internal class Matrix : ICloneable, IMatrixAttributes
    {
        public object Clone()
        {
            throw new NotImplementedException();
        }

        public ISavevalueAttributes Savevalue(int row, int column)
        {
            throw new NotImplementedException();
        }
    }
}
