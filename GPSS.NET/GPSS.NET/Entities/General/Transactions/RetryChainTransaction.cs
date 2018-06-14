using System;
using System.Collections.Generic;
using System.Text;
using GPSS.SimulationParts;

namespace GPSS.Entities.General.Transactions
{
    internal class RetryChainTransaction : TransactionDecorator
    {
        public RetryChainTransaction(Transaction innerTransaction, Func<bool> test, int? destinationBlockIndex) : base(innerTransaction)
        {
            Test = test;
            DestinationBlockIndex = destinationBlockIndex;
        }

        public Func<bool> Test { get; private set; }

        public int? DestinationBlockIndex { get; private set; }

        public void ReturnToCurrentEvents(TransactionScheduler scheduler)
        {
            if (DestinationBlockIndex.HasValue)
                InnerTransaction.NextBlock = DestinationBlockIndex.Value;

            scheduler.PlaceInCurrentEvents(InnerTransaction);
            scheduler.RemoveFromRetryChains(InnerTransaction);
        }
    }
}
