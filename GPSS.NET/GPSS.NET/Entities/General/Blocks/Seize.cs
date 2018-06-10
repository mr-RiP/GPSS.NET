using GPSS.Entities.Resources;
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

        public override void EnterBlock(Simulation simulation)
        {
            base.EnterBlock(simulation);
            try
            {
                var facility = simulation.Model.Resources.GetFacility(
                    FacilityName(simulation.StandardAttributes),
                    simulation.Scheduler);

                facility.Seize(simulation.Scheduler, simulation.ActiveTransaction.Transaction);
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
