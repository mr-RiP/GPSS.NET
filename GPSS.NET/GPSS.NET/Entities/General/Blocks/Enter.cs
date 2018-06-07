using GPSS.Entities.Resources;
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

        public override void EnterBlock(Simulation simulation)
        {
            try
            {
                base.EnterBlock(simulation);
                string name = StorageName(simulation.StandardAttributes);
                int capacity = StorageCapacity(simulation.StandardAttributes);
                simulation.Model.Resources.Storages[name]
                    .Enter(simulation.Scheduler, simulation.ActiveTransaction.Transaction, capacity);
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
    }
}
