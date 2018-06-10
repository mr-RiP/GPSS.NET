using System;
using System.Collections.Generic;
using System.Text;
using GPSS.Enums;
using GPSS.Exceptions;
using GPSS.ModelParts;

namespace GPSS.Entities.General.Blocks
{
    internal class Preempt : Block
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

        public override void EnterBlock(Simulation simulation)
        {
            try
            {   
                var facility = simulation.Model.Resources.GetFacility(
                    FacilityName(simulation.StandardAttributes), simulation.Scheduler);

                int? blockIndex = GetBlockIndex(simulation);
                bool removeMode = RemoveMode(simulation.StandardAttributes);
                if (blockIndex == null && removeMode)
                    throw new ModelStructureException(
                        "Attempt to preempt Facility entity im Remove Mode without " + 
                        "specifying new Next Block for current owner Transaction.",
                        simulation.ActiveTransaction.Transaction.CurrentBlock);
                var transaction = simulation.ActiveTransaction.Transaction;

                facility.Preempt(
                    simulation.Scheduler,
                    transaction,
                    PriorityMode(simulation.StandardAttributes),
                    ParameterName(simulation.StandardAttributes),
                    blockIndex,
                    removeMode);

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
            catch (ArgumentOutOfRangeException error)
            {
                throw new ModelStructureException(
                    "Attempt to redirect Facility Entity's owning Transaction to non-existing Block.",
                    simulation.ActiveTransaction.Transaction.CurrentBlock,
                    error);
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
    }
}
