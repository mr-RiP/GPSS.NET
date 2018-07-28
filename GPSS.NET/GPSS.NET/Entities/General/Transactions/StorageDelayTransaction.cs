using System;

namespace GPSS.Entities.General.Transactions
{
	internal class StorageDelayTransaction : TransactionDecorator
	{
		public StorageDelayTransaction(Transaction innerTransaction, int storageCapacity) : base(innerTransaction)
		{
			if (storageCapacity < 0)
				throw new ArgumentOutOfRangeException(nameof(storageCapacity));

			StorageCapacity = storageCapacity;
		}

		public int StorageCapacity { get; set; }

		public override Transaction Clone()
		{
			return new StorageDelayTransaction(InnerTransaction.Clone(), StorageCapacity);
		}
	}
}
