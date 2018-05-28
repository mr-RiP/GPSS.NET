using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.Groups
{
    class TransactionGroup : ICloneable, ITransactionGroupAttributes
    {
        public int Count => throw new NotImplementedException();

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
