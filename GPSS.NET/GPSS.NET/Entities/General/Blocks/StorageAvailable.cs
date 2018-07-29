using GPSS.Exceptions;
using System;

namespace GPSS.Entities.General.Blocks
{
    // http://www.minutemansoftware.com/reference/r7.htm#SAVAIL
    internal sealed class StorageAvailable : Block
    {
        private StorageAvailable()
        {

        }

        public StorageAvailable(Func<IStandardAttributes, string> storageName)
        {
            StorageName = storageName;
        }

        public Func<IStandardAttributes, string> StorageName { get; private set; }

        public override string TypeName => "SAVAIL";

        public override Block Clone() => new StorageAvailable
        {
            StorageName = StorageName,
            EntryCount = EntryCount,
            TransactionsCount = TransactionsCount,
        };

        public override void EnterBlock(Simulation simulation)
        {
            try
            {
                base.EnterBlock(simulation);
                string name = StorageName(simulation.StandardAttributes);

                // TODO: change this way for every storage
                var storage = GetResources(simulation).TryGetStorage(name);
                if (storage == null)
                {
                    throw new ModelStructureException(
                        "Storage entity with given name does not exists in thes Model",
                        GetCurrentBlockIndex(simulation));
                }
                storage.SetAvailable(simulation.Scheduler);
            }
            catch (ArgumentNullException error)
            {
                throw new ModelStructureException(
                    "Attempt to access Storage Entity by null name.",
                    GetCurrentBlockIndex(simulation),
                    error);
            }
        }
    }
}
