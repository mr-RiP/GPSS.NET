using System;
using System.Collections.Generic;
using System.Text;
using GPSS.SimulationParts;

namespace GPSS.Entities.General.Transactions
{
    internal class RetryChainTransaction : TransactionDecorator
    {
        public RetryChainTransaction(Transaction innerTransaction, Func<bool> test, int? destinationBlockIndex = null) : base(innerTransaction)
        {
            Test = test;
            NextBlock = destinationBlockIndex ?? InnerTransaction.NextBlock;
        }

        public Func<bool> Test { get; private set; }

        public override int NextBlock { get; set; }

        public void ReturnToCurrentEvents(TransactionScheduler scheduler)
        {
            InnerTransaction.NextBlock = NextBlock;

            scheduler.PlaceInCurrentEvents(InnerTransaction);
            scheduler.RemoveFromRetryChains(InnerTransaction);
        }
    }
}
