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
            try
            {
                string name = StorageName(simulation.StandardAttributes);
                int capacity = StorageCapacity(simulation.StandardAttributes);
                simulation.Model.Resources.Storages[name]
                    .Leave(simulation.Scheduler, capacity);
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
                    "Attempt to release more storage capacity than occupied.",
                    simulation.ActiveTransaction.Transaction.CurrentBlock,
                    error);
            }
        }
    }
}
