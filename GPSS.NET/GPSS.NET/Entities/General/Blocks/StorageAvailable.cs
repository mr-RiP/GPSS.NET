using GPSS.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General.Blocks
{
    // http://www.minutemansoftware.com/reference/r7.htm#SAVAIL
    internal class StorageAvailable : Block
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
            base.EnterBlock(simulation);

            string name = StorageName(simulation.StandardAttributes);
            if (name == null)
                throw new ModelStructureException(
                    "Attempt to access Storage Entity by null name.",
                    simulation.ActiveTransaction.Transaction.CurrentBlock,
                    new ArgumentNullException());

            if (simulation.Model.Resources.Storages.ContainsKey(name))
            {
                var storage = simulation.Model.Resources.Storages[name];
                storage.UpdateUsageHistory(simulation.System);
                storage.Available = true;
                storage.MoveChain(simulation.Chains);
            }
            else
                throw new ModelStructureException(
                    "Storage entity with given name does not exists in the Model.",
                    simulation.ActiveTransaction.Transaction.CurrentBlock);
        }
    }
}
