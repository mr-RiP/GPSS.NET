﻿using GPSS.Entities.General.Transactions;
using GPSS.Enums;
using GPSS.Exceptions;
using System;

namespace GPSS.Entities.General.Blocks
{
	internal class Seize : Block
	{
		private Seize()
		{

		}

		public Seize(Func<IStandardAttributes, string> facilityName)
		{
			FacilityName = facilityName;
		}

		public Func<IStandardAttributes, string> FacilityName { get; private set; }

		public override string TypeName => "SEIZE";

		public override bool CanEnter(Simulation simulation)
		{
			try
			{
				var facility = simulation.Model.Resources.GetFacility(
					FacilityName(simulation.StandardAttributes),
					simulation.Scheduler);

				bool allow = facility.Available && facility.Idle;
				if (!allow)
					simulation.ActiveTransaction.Transaction.Delayed = true;

				return allow;

			}
			catch (ArgumentNullException error)
			{
				throw new ModelStructureException(
					"Attempt to access Facility Entity by null name.",
					simulation.ActiveTransaction.Transaction.CurrentBlock,
					error);
			}
		}

		public override void EnterBlock(Simulation simulation)
		{
			try
			{
				var facility = simulation.Model.Resources.GetFacility(
					FacilityName(simulation.StandardAttributes),
					simulation.Scheduler);
				var transaction = simulation.ActiveTransaction.Transaction;

				facility.Seize(simulation.Scheduler, transaction);
				if (transaction.State == TransactionState.Active)
					base.EnterBlock(simulation);
			}
			catch (ArgumentNullException error)
			{
				throw new ModelStructureException(
					"Attempt to access Facility Entity by null name.",
					simulation.ActiveTransaction.Transaction.CurrentBlock,
					error);
			}
		}

		public override Block Clone() => new Seize
		{
			FacilityName = FacilityName,
			EntryCount = EntryCount,
			TransactionsCount = TransactionsCount,
		};

		public override void AddRetry(Simulation simulation, int? destinationBlockIndex = null)
		{
			var transaction = simulation.ActiveTransaction.Transaction;
			var name = FacilityName(simulation.StandardAttributes);
			var facility = simulation.Model.Resources.Facilities[name];

			facility.RetryChain.AddLast(new RetryChainTransaction(
				transaction,
				() => facility.Available && facility.Idle,
				destinationBlockIndex));
		}
	}
}
