using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.Calculations
{
    internal class Matrix : ICloneable, IMatrixAttributes
    {
        public Matrix Clone()
        {
            throw new NotImplementedException();
        }

        object ICloneable.Clone() => Clone();

        public ISavevalueAttributes Savevalue(int row, int column)
        {
            throw new NotImplementedException();
        }

        internal void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
