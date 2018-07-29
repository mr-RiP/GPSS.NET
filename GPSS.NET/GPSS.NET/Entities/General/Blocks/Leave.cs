using GPSS.Exceptions;
using System;

namespace GPSS.Entities.General.Blocks
{
    // http://www.minutemansoftware.com/reference/r7.htm#LEAVE
    internal sealed class Leave : Block
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
            try
            {
                string name = StorageName(simulation.StandardAttributes);
                int capacity = StorageCapacity(simulation.StandardAttributes);

                if (!simulation.Model.Resources.Storages.TryGetValue(name, out var storage))
                {
                    throw new ModelStructureException(
                        "Storage entity with given name does not exists in thes Model",
                        GetCurrentBlockIndex(simulation));
                }
                storage.Leave(simulation.Scheduler, capacity);
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
                    "Attempt to release more storage capacity than occupied.",
                    GetCurrentBlockIndex(simulation),
                    error);
            }
        }
    }
}
