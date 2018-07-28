using GPSS.Entities.General.Transactions;
using System.Collections.Generic;

namespace GPSS.Extensions
{
	internal interface IRetryChainContainer
	{
		LinkedList<RetryChainTransaction> RetryChain { get; }
	}
}
