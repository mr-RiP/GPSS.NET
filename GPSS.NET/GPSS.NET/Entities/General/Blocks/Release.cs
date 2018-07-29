using GPSS.Exceptions;
using System;

namespace GPSS.Entities.General.Blocks
{
	internal sealed class Release : Block
	{
		private Release()
		{

		}

		public Release(Func<IStandardAttributes, string> facilityName)
		{
			FacilityName = facilityName;
		}

		public Func<IStandardAttributes, string> FacilityName { get; private set; }

		public override string TypeName => "RELEASE";

		public override void EnterBlock(Simulation simulation)
		{
			base.EnterBlock(simulation);
			try
			{
				var facility = simulation.Model.Resources.GetFacility(
					FacilityName(simulation.StandardAttributes),
					simulation.Scheduler);

				facility.Release(simulation.Scheduler, simulation.ActiveTransaction.Transaction);
			}
			catch (ArgumentNullException error)
			{
				throw new ModelStructureException(
					"Attempt to access Facility Entity by null name.",
					GetCurrentBlockIndex(simulation),
					error);
			}
			catch (ArgumentOutOfRangeException error)
			{
				throw new ModelStructureException(
					"Attempt to release Facility entity from Transaction not owning or preempted from it.",
					GetCurrentBlockIndex(simulation),
					error);
			}
		}

		public override Block Clone() => new Release
		{
			FacilityName = FacilityName,
			EntryCount = EntryCount,
			TransactionsCount = TransactionsCount,
		};
	}
}
