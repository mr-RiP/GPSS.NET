using GPSS.Exceptions;
using System;
using System.Collections.Generic;

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
			try
			{
				base.EnterBlock(simulation);
				string name = StorageName(simulation.StandardAttributes);
				simulation.Model.Resources.Storages[name].SetAvailable(simulation.Scheduler);
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
