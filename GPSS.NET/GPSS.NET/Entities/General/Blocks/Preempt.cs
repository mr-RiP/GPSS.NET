using GPSS.Entities.General.Transactions;
using GPSS.Enums;
using GPSS.Exceptions;
using System;

namespace GPSS.Entities.General.Blocks
{
    internal sealed class Preempt : Block
    {
        private Preempt()
        {

        }

        public Preempt(
            Func<IStandardAttributes, string> facilityName,
            Func<IStandardAttributes, bool> priorityMode,
            Func<IStandardAttributes, string> newNextBlock,
            Func<IStandardAttributes, string> parameterName,
            Func<IStandardAttributes, bool> removeMode)
        {
            FacilityName = facilityName;
            PriorityMode = priorityMode;
            NewNextBlock = newNextBlock;
            ParameterName = parameterName;
            RemoveMode = removeMode;
        }

        public Func<IStandardAttributes, string> FacilityName { get; private set; }
        public Func<IStandardAttributes, bool> PriorityMode { get; private set; }
        public Func<IStandardAttributes, string> NewNextBlock { get; private set; }
        public Func<IStandardAttributes, string> ParameterName { get; private set; }
        public Func<IStandardAttributes, bool> RemoveMode { get; private set; }

        public override string TypeName => "PREEMPT";

        public override bool CanEnter(Simulation simulation)
        {
            try
            {
                var transaction = simulation.ActiveTransaction.Transaction;
                var facility = simulation.Model.Resources.GetFacility(
                    FacilityName(simulation.StandardAttributes), simulation.Scheduler);
                bool priorityMode = PriorityMode(simulation.StandardAttributes);

                bool allow = facility.Available &&
                    (facility.Idle ||
                    (priorityMode && transaction.Priority > facility.Owner.Priority) ||
                    (!facility.Interrupted));

                if (!allow)
                    transaction.Delayed = true;

                return allow;
            }
            catch (ArgumentNullException error)
            {
                throw new ModelStructureException(
                    "Attempt to access Facility Entity by null name.",
                    GetCurrentBlockIndex(simulation),
                    error);
            }
        }

        public override void EnterBlock(Simulation simulation)
        {
            var transaction = simulation.ActiveTransaction.Transaction;
            try
            {
                var scheduler = simulation.Scheduler;
                var facility = simulation.Model.Resources.GetFacility(FacilityName(simulation.StandardAttributes), scheduler);

                int? blockIndex = GetBlockIndex(simulation);
                bool removeMode = RemoveMode(simulation.StandardAttributes);
                if (blockIndex == null && removeMode)
                    throw new ModelStructureException(
                        "Attempt to preempt Facility entity im Remove Mode without " +
                        "specifying new Next Block for current owner Transaction.",
                        transaction.CurrentBlock);

                facility.Preempt(scheduler, transaction,
                    PriorityMode(simulation.StandardAttributes),
                    ParameterName(simulation.StandardAttributes),
                    blockIndex, removeMode);

                if (simulation.ActiveTransaction.Transaction.State == TransactionState.Active)
                    base.EnterBlock(simulation);
            }
            catch (ArgumentNullException error)
            {
                throw new ModelStructureException(
                    "Attempt to access Facility Entity by null name.",
                    transaction.CurrentBlock, error);
            }
            catch (ArgumentOutOfRangeException error)
            {
                throw new ModelStructureException(
                    "Attempt to redirect Facility Entity's owning Transaction to non-existing Block.",
                    transaction.CurrentBlock, error);
            }
        }

        private int? GetBlockIndex(Simulation simulation)
        {
            string name = NewNextBlock(simulation.StandardAttributes);
            if (name != null)
                return simulation.Model.Statements.Labels[name];
            else
                return null;
        }

        public override Block Clone() => new Preempt
        {
            FacilityName = FacilityName,
            PriorityMode = PriorityMode,
            NewNextBlock = NewNextBlock,
            ParameterName = ParameterName,
            RemoveMode = RemoveMode,

            EntryCount = EntryCount,
            TransactionsCount = TransactionsCount,
        };

        public override void AddRetry(Simulation simulation, int? destinationBlockIndex = null)
        {
            var transaction = simulation.ActiveTransaction.Transaction;
            var name = FacilityName(simulation.StandardAttributes);
            var facility = simulation.Model.Resources.Facilities[name];
            bool priorityMode = PriorityMode(simulation.StandardAttributes);

            bool Test()
            {
                return facility.Available &&
                    (facility.Idle ||
                    (priorityMode && transaction.Priority > facility.Owner.Priority) ||
                    (!facility.Interrupted));
            }

            facility.RetryChain.AddLast(new RetryChainTransaction(transaction, Test, destinationBlockIndex));
        }
    }
}
