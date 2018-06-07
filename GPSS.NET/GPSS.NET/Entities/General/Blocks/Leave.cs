using GPSS.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General.Blocks
{
    // http://www.minutemansoftware.com/reference/r7.htm#LEAVE
    internal class Leave : Block
    {
        private Leave()
        {

        }

        public Leave(Func<IStandardAttributes, string> storageName, Func<IStandardAttributes, int> storageCapacity)
        {
            StorageName = storageName;
            StorageCapacity = storageCapacity;
        }

        public Func<IStandardAttributes, string> StorageName { get; private set; }

        public Func<IStandardAttributes, int> StorageCapacity { get; private set; }

        public override string TypeName => "LEAVE";

        public override Block Clone() => new Leave
        {
            StorageName = StorageName,
            StorageCapacity = StorageCapacity,

            EntryCount = EntryCount,
            TransactionsCount = TransactionsCount,
        };

        public override void EnterBlock(Simulation simulation)
        {
            base.EnterBlock(simulation);

            string name = StorageName(simulation.StandardAttributes);
            if (name == null)
                throw new ModelStructureException(
                    "Attempt to access Storage Entity by null name.",
                    simulation.ActiveTransaction.Transaction.CurrentBlock,
                    new ArgumentNullException());

            if (simulation.Model.Resources.Storages.ContainsKey(name))
            {
                int capacity = StorageCapacity(simulation.StandardAttributes);
                if (capacity < 0)
                    throw new ModelStructureException(
                        "Attempt to release negative value of storage capacity.",
                        simulation.ActiveTransaction.Transaction.CurrentBlock);

                var storage = simulation.Model.Resources.Storages[name];
                if (capacity > storage.OccupiedCapacity)
                    throw new ModelStructureException(
                        "Attempt to release more storage capacity than occupied.",
                        simulation.ActiveTransaction.Transaction.CurrentBlock);

                storage.UpdateUsageHistory(simulation.Scheduler);
                storage.Leave(capacity);

                if (storage.Available)
                    storage.MoveDelayChain(simulation.Scheduler);
            }
            else
                throw new ModelStructureException(
                    "Storage entity with given name does not exists in the Model.",
                    simulation.ActiveTransaction.Transaction.CurrentBlock);
        }
    }
}
