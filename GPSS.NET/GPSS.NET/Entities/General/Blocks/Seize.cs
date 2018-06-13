using GPSS.Entities.Resources;
using GPSS.Enums;
using GPSS.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
