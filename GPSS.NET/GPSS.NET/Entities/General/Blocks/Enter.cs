using GPSS.Entities.General.Transactions;
using GPSS.Entities.Resources;
using GPSS.Enums;
using GPSS.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General.Blocks
{
    // http://www.minutemansoftware.com/reference/r7.htm#ENTER
    internal class Enter : Block
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
                    simulation.ActiveTransaction.Transaction.CurrentBlock);

                var storage = simulation.Model.Resources.Storages[name];

                bool allow = storage.Available && storage.AvailableCapacity >= capacity;
                if (!allow)
                    simulation.ActiveTransaction.Transaction.Delayed = true;

                return allow;
            }
            catch (ArgumentNullException error)
            {
                throw new ModelStructureException(
                    "Attempt to access Storage Entity by null name.",
                    simulation.ActiveTransaction.Transaction.CurrentBlock,
                    error);
            }
        }

        public override void EnterBlock(Simulation simulation)
        {
            try
            {
                base.EnterBlock(simulation);
                string name = StorageName(simulation.StandardAttributes);
                var transaction = simulation.ActiveTransaction.Transaction;
                int capacity = StorageCapacity(simulation.StandardAttributes);
                if (capacity <= 0)
                    throw new ModelStructureException(
                    "Attempt to occupy non-positive number of Storage Capacity Units.",
                    simulation.ActiveTransaction.Transaction.CurrentBlock);

                simulation.Model.Resources.Storages[name]
                    .Enter(simulation.Scheduler, transaction, capacity);

                if (transaction.State == TransactionState.Active)
                    base.EnterBlock(simulation);
            }
            catch (ArgumentNullException error)
            {
                throw new ModelStructureException(
                    "Attempt to access Storage Entity by null name.",
                    simulation.ActiveTransaction.Transaction.CurrentBlock,
                    error);
            }
            catch (KeyNotFoundException error)
            {
                throw new ModelStructureException(
                    "Storage entity with given name does not exists in thes Model.",
                    simulation.ActiveTransaction.Transaction.CurrentBlock,
                    error);
            }
            catch (ArgumentOutOfRangeException error)
            {
                throw new ModelStructureException(
                    "Attempt to occupy more storage capacity than total capacity of given Storage Entity.",
                    simulation.ActiveTransaction.Transaction.CurrentBlock,
                    error);
            }
        }

        public override void AddRetry(Simulation simulation, RetryChainTransaction retry)
        {
            var name = StorageName(simulation.StandardAttributes);
            simulation.Model.Resources.Storages[name].RetryChain.AddLast(retry);
        }
    }
}
