using GPSS.Entities.General.Transactions;
using GPSS.StandardAttributes;
using System;

namespace GPSS.Entities.General
{
	internal abstract class Block : ICloneable, IBlockAttributes
	{
		abstract public string TypeName { get; }

		public int EntryCount { get; protected set; }

		public int TransactionsCount { get; protected set; }

		abstract public Block Clone();
		object ICloneable.Clone() => Clone();

		public virtual int RetryCount { get => 0; }
		public virtual bool RemoveRetry(Simulation simulation, RetryChainTransaction retry)
		{
			return false;
		}

		public virtual void AddRetry(Simulation simulation, int? destinationBlockIndex = null)
		{
		}

		public virtual void EnterBlock(Simulation simulation)
		{
			EntryCount++;
			TransactionsCount++;
			var transaction = simulation.ActiveTransaction.Transaction;
			if (transaction.CurrentBlock >= 0)
				simulation.Model.Statements.Blocks[transaction.CurrentBlock].TransactionsCount--;
			transaction.CurrentBlock = transaction.NextBlock;
			transaction.NextBlock++;
		}

		public virtual bool CanEnter(Simulation simulation)
		{
			return true;
		}
	}
}
