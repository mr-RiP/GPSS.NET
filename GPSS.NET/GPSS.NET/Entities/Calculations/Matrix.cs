﻿using GPSS.StandardAttributes;
using System;

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

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
