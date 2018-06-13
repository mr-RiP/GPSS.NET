using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General.Transactions.RetryChainTransactions
{
    // TODO
    internal class TestRetry : RetryChainTransaction
    {
        public TestRetry(Transaction innerTransaction) : base(innerTransaction)
        {
        }

        public override bool Resolve(Simulation simulation)
        {
            throw new NotImplementedException();
        }
    }
}
