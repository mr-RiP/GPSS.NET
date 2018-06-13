using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General.Transactions
{
    abstract internal class RetryChainTransaction : TransactionDecorator
    {
        public RetryChainTransaction(Transaction innerTransaction) : base(innerTransaction)
        {
        }

        // Transfer - не может войти в блок?
        // Test - 2 сущности не соответствуют условию?
        // Gate - 1 сущность не соответствует условию?
        abstract public bool Resolve(Simulation simulation);
    }
}
