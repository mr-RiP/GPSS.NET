using GPSS.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General.Blocks
{
    // http://www.minutemansoftware.com/reference/r7.htm#SUNAVAIL
    internal class StorageUnavailable : Block
    {
        private StorageUnavailable()
        {

        }

        public StorageUnavailable(Func<IStandardAttributes, string> storageName)
        {
            StorageName = storageName;
        }

        public Func<IStandardAttributes, string> StorageName { get; private set; }

        public override string TypeName => "SUNAVAIL";

        public override Block Clone() => new StorageUnavailable
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
                simulation.Model.Resources.Storages[name].SetUnavailable();
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
        }
    }
}
