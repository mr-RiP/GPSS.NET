using GPSS.Enums;
using GPSS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPSS.Entities.General.Blocks
{
    internal class Terminate : Block
    {
        private Terminate()
        {

        }

        public Terminate(Func<IStandardAttributes, int> terminationDecrement)
        {
            TerminationDecrement = terminationDecrement;
        }

        public Func<IStandardAttributes, int> TerminationDecrement { get; private set; } 

        public override string TypeName => "TERMINATE";

        public override Block Clone() => new Terminate
        {
            TerminationDecrement = TerminationDecrement,
            EntryCount = EntryCount,
            TransactionsCount = TransactionsCount,
        };

        public override void EnterBlock(Simulation simulation)
        {
            base.EnterBlock(simulation);

            var transaction = simulation.ActiveTransaction.Transaction;
            transaction.State = TransactionState.Terminated;

            int decrement = TerminationDecrement(simulation.StandardAttributes);
            if (decrement < 0)
                throw new ModelStructureException("Termination decrement value must be positive or zero.",
                    transaction.CurrentBlock);

            if (transaction.Preempted)
                throw new ModelStructureException("Attempt to Terminate transaction preempted from Facility.",
                    transaction.CurrentBlock);

            if (simulation.Model.Resources.Facilities.Values.Any(f => f.Owner == transaction))
                throw new ModelStructureException("Attempt to Terminate transaction currently owning Facility.",
                    transaction.CurrentBlock);

            simulation.Scheduler.CurrentEvents.Remove(transaction);
            simulation.Scheduler.TerminationCount -= decrement;

            TransactionsCount--;
        }
    }
}
