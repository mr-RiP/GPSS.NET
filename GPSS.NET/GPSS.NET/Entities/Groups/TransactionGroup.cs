using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.Groups
{
    internal class TransactionGroup : ICloneable, ITransactionGroupAttributes
    {
        public int Count => throw new NotImplementedException();

        public TransactionGroup Clone()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        object ICloneable.Clone() => Clone();
    }
}
