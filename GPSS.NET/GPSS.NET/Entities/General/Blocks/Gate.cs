using GPSS.Entities.General.Transactions;
using GPSS.Enums;
using GPSS.Exceptions;
using GPSS.Extensions;
using System;
using System.Collections.Generic;

namespace GPSS.Entities.General.Blocks
{
    internal sealed class Gate : Block
    {
        private Gate()
        {
        }

        public Gate(
            Func<IStandardAttributes, GateCondition> condition,
            Func<IStandardAttributes, string> entityName,
            Func<IStandardAttributes, string> alternateDestination)
        {
            Condition = condition;
            EntityName = entityName;
            AlternateDestination = alternateDestination;
        }

        public Func<IStandardAttributes, GateCondition> Condition { get; set; }
        public Func<IStandardAttributes, string> EntityName { get; set; }
        public Func<IStandardAttributes, string> AlternateDestination { get; set; }

        public override string TypeName => "GATE";

        public override bool CanEnter(Simulation simulation)
        {
            try
            {
                return AlternateDestination(simulation.StandardAttributes) != null;
            }
            catch (ArgumentNullException error)
            {
                throw new ModelStructureException(
                    "Attempt to perform test on entity with null name.",
                    GetCurrentBlockIndex(simulation),
                    error);
            }
            catch (KeyNotFoundException error)
            {
                throw new ModelStructureException(
                    "Attempt to perform test on non-existing entity.",
                    GetCurrentBlockIndex(simulation),
                    error);
            }
        }

        public override void EnterBlock(Simulation simulation)
        {
            try
            {
                Test(simulation);
            }
            catch (ArgumentNullException error)
            {
                throw new ModelStructureException(
                    "Attempt to perform test on entity with null name.",
                    simulation.ActiveTransaction.Transaction.NextBlock,
                    error);
            }
            catch (KeyNotFoundException error)
            {
                throw new ModelStructureException(
                    "Attempt to perform test on non-existing entity.",
                    simulation.ActiveTransaction.Transaction.NextBlock,
                    error);
            }
        }

        private void Test(Simulation simulation)
        {
            var condition = Condition(simulation.StandardAttributes);
            var testObject = GetTestObject(simulation, condition);
            Func<bool> test = condition.GetTest(testObject);
            if (test())
                base.EnterBlock(simulation);
            else
            {
                string alternateDestination = AlternateDestination(simulation.StandardAttributes);
                if (alternateDestination == null)
                    RefuseMode(simulation, testObject, test);
                else
                    AlternateDestinationMode(simulation, alternateDestination);
            }
        }

        private IRetryChainContainer GetTestObject(Simulation simulation, GateCondition condition)
        {
            var entityType = condition.GetEntityType();
            var name = EntityName(simulation.StandardAttributes);
            switch (entityType)
            {
                case EntityType.Facility:
                    return simulation.Model.Resources.GetFacility(name, simulation.Scheduler);
                case EntityType.Storage:
                    bool storageExists = simulation.Model.Resources.Storages.TryGetValue(name, out var storage);
                    return storageExists ? storage :
                        throw new ModelStructureException(
                        "Storage entity with given name does not exists in thes Model",
                        GetCurrentBlockIndex(simulation));

                default:
                    throw new NotImplementedException();
            }
        }

        private void AlternateDestinationMode(Simulation simulation, string alternateDestination)
        {
            try
            {
                base.EnterBlock(simulation);
                int index = simulation.Model.Statements.Labels[alternateDestination];
                simulation.ActiveTransaction.Transaction.NextBlock = index;
            }
            catch (KeyNotFoundException error)
            {
                throw new ModelStructureException(
                    "Attempt to move transaction to non-existing Block.",
                    GetCurrentBlockIndex(simulation),
                    error);
            }
        }

        private void RefuseMode(Simulation simulation, IRetryChainContainer container, Func<bool> test)
        {
            var transaction = simulation.ActiveTransaction.Transaction;

            transaction.State = TransactionState.Suspended;
            simulation.Scheduler.CurrentEvents.Remove(transaction);

            container.RetryChain.AddLast(new RetryChainTransaction(transaction, test));
        }

        public override Block Clone() => new Gate
        {
            Condition = Condition,
            EntityName = EntityName,
            AlternateDestination = AlternateDestination,

            EntryCount = EntryCount,
            TransactionsCount = TransactionsCount,
        };
    }
}
