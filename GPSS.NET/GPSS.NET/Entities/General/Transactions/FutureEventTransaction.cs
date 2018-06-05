using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General.Transactions
{
    internal class FutureEventTransaction : TransactionDecorator
    {
        public FutureEventTransaction(Transaction innerTransaction, double timeIncrement) : base(innerTransaction)
        {
            if (timeIncrement <= 0.0)
                throw new ArgumentOutOfRangeException(nameof(timeIncrement));

            TimeIncrement = timeIncrement;
        }

        public double TimeIncrement { get; set; }

        public override Transaction Clone()
        {
            return new FutureEventTransaction(InnerTransaction.Clone(), TimeIncrement);
        }
    }
}
