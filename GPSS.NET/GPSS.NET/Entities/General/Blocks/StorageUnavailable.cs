using GPSS.Exceptions;
using System;

namespace GPSS.Entities.General.Blocks
{
	// http://www.minutemansoftware.com/reference/r7.htm#SUNAVAIL
	internal sealed class StorageUnavailable : Block
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

				var storage = GetResources(simulation).TryGetStorage(name);
				if (storage == null)
				{
					throw new ModelStructureException(
						"Storage entity with given name does not exists in thes Model",
						GetCurrentBlockIndex(simulation));
				}
				storage.SetUnavailable();
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
