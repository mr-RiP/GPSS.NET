using GPSS.Entities.General.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Extensions
{
    internal interface IRetryChainContainer
    {
        LinkedList<RetryChainTransaction> RetryChain { get; }
    }
}
