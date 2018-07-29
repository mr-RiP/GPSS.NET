using GPSS.Entities.General.Transactions;
using GPSS.Enums;
using GPSS.Exceptions;
using System;

namespace GPSS.Entities.General.Blocks
{
	// http://www.minutemansoftware.com/reference/r7.htm#ENTER
	internal sealed class Enter : Block
	{
		private Enter()
		{

		}

		public Enter(Func<IStandardAttributes, string> storageName, Func<IStandardAttributes, int> storageCapacity)
		{
			StorageName = storageName;
			StorageCapacity = storageCapacity;
		}

		public Func<IStandardAttributes, string> StorageName { get; private set; }

		public Func<IStandardAttributes, int> StorageCapacity { get; private set; }

		public override string TypeName => "ENTER";

		public override Block Clone() => new Enter
		{
			StorageName = StorageName,
			StorageCapacity = StorageCapacity,

			EntryCount = EntryCount,
			TransactionsCount = TransactionsCount,
		};

		public override bool CanEnter(Simulation simulation)
		{
			try
			{
				string name = StorageName(simulation.StandardAttributes);
				int capacity = StorageCapacity(simulation.StandardAttributes);
				if (capacity <= 0)
					throw new ModelStructureException(
					"Attempt to occupy non-positive number of Storage Capacity Units.",
					GetCurrentBlockIndex(simulation));

				var storage = GetResources(simulation).TryGetStorage(name);
				if (storage == null)
				{
					throw new ModelStructureException(
						"Storage entity with given name does not exists in thes Model",
						GetCurrentBlockIndex(simulation));
				}

				bool allow = storage.Available && storage.AvailableCapacity >= capacity;
				if (!allow)
					simulation.ActiveTransaction.Transaction.Delayed = true;

				return allow;
			}
			catch (ArgumentNullException error)
			{
				throw new ModelStructureException(
					"Attempt to access Storage Entity by null name.",
					GetCurrentBlockIndex(simulation),
					error);
			}
		}

		public override void EnterBlock(Simulation simulation)
		{
			try
			{
				string name = StorageName(simulation.StandardAttributes);
				var transaction = simulation.ActiveTransaction.Transaction;
				int capacity = StorageCapacity(simulation.StandardAttributes);
				if (capacity <= 0)
					throw new ModelStructureException(
					"Attempt to occupy non-positive number of Storage Capacity Units.",
					GetCurrentBlockIndex(simulation));

				if (!simulation.Model.Resources.Storages.TryGetValue(name, out var storage))
				{
					throw new ModelStructureException(
						"Storage entity with given name does not exists in thes Model",
						GetCurrentBlockIndex(simulation));
				}
				storage.Enter(simulation.Scheduler, transaction, capacity);

				if (transaction.State == TransactionState.Active)
					base.EnterBlock(simulation);
			}
			catch (ArgumentNullException error)
			{
				throw new ModelStructureException(
					"Attempt to access Storage Entity by null name.",
					GetCurrentBlockIndex(simulation),
					error);
			}
			catch (ArgumentOutOfRangeException error)
			{
				throw new ModelStructureException(
					"Attempt to occupy more storage capacity than total capacity of given Storage Entity.",
					GetCurrentBlockIndex(simulation),
					error);
			}
		}

		public override void AddRetry(Simulation simulation, int? destinationBlockIndex = null)
		{
			var transaction = simulation.ActiveTransaction.Transaction;

			string name = StorageName(simulation.StandardAttributes);
			if (!simulation.Model.Resources.Storages.TryGetValue(name, out var storage))
			{
				throw new ModelStructureException(
					"Storage entity with given name does not exists in thes Model",
					GetCurrentBlockIndex(simulation));
			}
			int capacity = StorageCapacity(simulation.StandardAttributes);

			storage.RetryChain.AddLast(new RetryChainTransaction(
				transaction,
				(() => storage.Available && storage.AvailableCapacity >= capacity),
				destinationBlockIndex));
		}
	}
}
