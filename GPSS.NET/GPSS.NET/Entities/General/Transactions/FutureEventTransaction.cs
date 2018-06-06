using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General.Transactions
{
    internal class FutureEventTransaction : TransactionDecorator
    {
        public FutureEventTransaction(Transaction innerTransaction, double releaseTime) : base(innerTransaction)
        {
            if (releaseTime <= 0.0)
                throw new ArgumentOutOfRangeException(nameof(releaseTime));

            ReleaseTime = releaseTime;
        }

        public double ReleaseTime { get; set; }

        public override Transaction Clone()
        {
            return new FutureEventTransaction(InnerTransaction.Clone(), ReleaseTime);
        }
    }
}
