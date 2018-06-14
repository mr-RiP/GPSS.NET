using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General.Transactions
{
    internal class FutureEventTransaction : TransactionDecorator
    {
        public FutureEventTransaction(Transaction innerTransaction, double departureTime) : base(innerTransaction)
        {
            if (departureTime <= 0.0)
                throw new ArgumentOutOfRangeException(nameof(departureTime));

            DepartureTime = departureTime;
        }

        public double DepartureTime { get; set; }

        public override Transaction Clone()
        {
            return new FutureEventTransaction(InnerTransaction.Clone(), DepartureTime);
        }
    }
}
