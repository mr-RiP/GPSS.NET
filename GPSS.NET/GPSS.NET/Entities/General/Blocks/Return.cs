using GPSS.Exceptions;
using System;

namespace GPSS.Entities.General.Blocks
{
	internal class Return : Block
	{
		private Return()
		{

		}

		public Return(Func<IStandardAttributes, string> facilityName)
		{
			FacilityName = facilityName;
		}

		public Func<IStandardAttributes, string> FacilityName { get; private set; }

		public override string TypeName => "RETURN";

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
					simulation.ActiveTransaction.Transaction.CurrentBlock,
					error);
			}
			catch (ArgumentOutOfRangeException error)
			{
				throw new ModelStructureException(
					"Attempt to release Facility entity from Transaction not owning or preempted from it.",
					simulation.ActiveTransaction.Transaction.CurrentBlock,
					error);
			}
		}

		public override Block Clone() => new Return
		{
			FacilityName = FacilityName,
			EntryCount = EntryCount,
			TransactionsCount = TransactionsCount,
		};
	}
}
