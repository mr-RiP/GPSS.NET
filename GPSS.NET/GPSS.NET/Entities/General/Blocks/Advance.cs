using GPSS.Enums;
using GPSS.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General.Blocks
{
    internal class Advance : Block
    {
        private Advance()
        {

        }

        public Advance(Func<IStandardAttributes, double> delay)
        {
            Delay = delay;
        }

        public Func<IStandardAttributes, double> Delay { get; private set; }

        public override string TypeName => "ADVANCE";

        public override Block Clone() => new Advance
        {
            Delay = Delay,
            EntryCount = EntryCount,
            TransactionsCount = TransactionsCount,
        };

        public override void EnterBlock(Simulation simulation)
        {
            base.EnterBlock(simulation);

            var transaction = simulation.ActiveTransaction.Transaction;
            transaction.State = TransactionState.Suspended;

            double time = Delay(simulation.StandardAttributes);
            if (time < 0.0)
                throw new ModelStructureException("Negative time increment.", transaction.CurrentBlock);

            var chains = simulation.Scheduler;
            chains.CurrentEvents.Remove(transaction);
            if (time == 0.0)
                chains.PlaceInCurrentEvents(transaction);
            else if (transaction.Preempted)
            {
                transaction.NextBlock = transaction.CurrentBlock;
                chains.PlaceInCurrentEvents(transaction);
            }
            else
                chains.PlaceInFutureEvents(transaction, time);

        }
    }
}
