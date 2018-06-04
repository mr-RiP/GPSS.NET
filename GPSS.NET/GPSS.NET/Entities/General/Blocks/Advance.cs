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

        public override void Run(Simulation simulation)
        {
            EnterBlock();
            double time = Delay(simulation.StandardAttributes);
            if (time < 0.0)
                throw new ModelStructureException("Negative time increment.", GetBlockIndex(simulation));

            var transaction = simulation.ActiveTransaction.Transaction;
            var chains = simulation.Chains;

            transaction.Chain = TransactionState.Suspended;
            chains.CurrentEvents.Remove(transaction);
            if (time == 0.0)
                chains.PlaceInCurrentEvents(transaction);
            else
                chains.PlaceInFutureEvents(transaction);
            ExitBlock();
        }
    }
}
