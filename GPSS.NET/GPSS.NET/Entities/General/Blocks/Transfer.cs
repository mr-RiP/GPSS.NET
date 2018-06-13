using GPSS.Entities.General.RetryChain;
using GPSS.Enums;
using GPSS.Exceptions;
using GPSS.Extensions;
using GPSS.ModelParts;
using GPSS.SimulationParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPSS.Entities.General.Blocks
{
    internal class Transfer : Block, IRetryChainContainer
    {
        private Transfer()
        {

        }

        public Transfer(
            Func<IStandardAttributes, TransferMode> mode,
            Func<IStandardAttributes, string> primaryDestination,
            Func<IStandardAttributes, string> secondaryDestination,
            Func<IStandardAttributes, int> increment)
        {
            Mode = mode;
            PrimaryDestination = primaryDestination;
            SecondaryDestination = secondaryDestination;
            Increment = increment;
        }

        public Func<IStandardAttributes, TransferMode> Mode { get; private set; }
        public Func<IStandardAttributes, double> Fraction { get; private set; }
        public Func<IStandardAttributes, string> PrimaryDestination { get; private set; }
        public Func<IStandardAttributes, string> SecondaryDestination { get; private set; }
        public Func<IStandardAttributes, int> Increment { get; private set; }

        public Dictionary<Transaction, TransferTestData> RetryChain { get; private set; } = new Dictionary<Transaction, TransferTestData>();
        public override int RetryCount => RetryChain.Count;

        public override string TypeName => "TRANSFER";

        public override void EnterBlock(Simulation simulation)
        {
            var transaction = simulation.ActiveTransaction.Transaction;
            try
            {
                if (Contains(transaction))
                {
                    if (Resolve(simulation, transaction, RetryChain[transaction]))
                        Remove(transaction);
                    else
                        Reset(simulation.Scheduler, transaction);
                }
                else
                {
                    base.EnterBlock(simulation);
                    var data = CalculateTestData(simulation);
                    if (!Resolve(simulation, transaction, data))
                    {
                        Add(transaction, data);
                        Reset(simulation.Scheduler, transaction);
                    }
                }
            }
            catch (ArgumentNullException error)
            {
                throw new ModelStructureException(
                    "Attempt to perform transfer test with null argument.",
                    transaction.CurrentBlock,
                    error);
            }
            catch (KeyNotFoundException error)
            {
                throw new ModelStructureException(
                    "Attempt to perform transfer test with non existent Block name.",
                    transaction.CurrentBlock,
                    error);
            }
        }

        private TransferTestData CalculateTestData(Simulation simulation)
        {
            return new TransferTestData {
                Mode = Mode(simulation.StandardAttributes),
                PrimaryDestination = PrimaryDestination(simulation.StandardAttributes),
                SecondaryDestination = SecondaryDestination(simulation.StandardAttributes),
                Increment = Increment(simulation.StandardAttributes),
            };
        }

        private void Add(Transaction transaction, TransferTestData data)
        {
            transaction.NextBlock = transaction.CurrentBlock;
            RetryChain.Add(transaction, data);
        }

        private void Reset(TransactionScheduler scheduler, Transaction transaction)
        {
            transaction.State = TransactionState.Suspended;
            scheduler.CurrentEvents.Remove(transaction);
            scheduler.PlaceInCurrentEvents(transaction);
        }

        private bool Resolve(Simulation simulation, Transaction transaction, TransferTestData testData)
        {
            switch (testData.Mode)
            {
                case TransferMode.Unconditional:
                    return TransferUnconditional(simulation, transaction, testData);
                case TransferMode.Fractional:
                    return TransferFractional(simulation, transaction, testData);
                case TransferMode.Both:
                    return TranferBoth(simulation, transaction, testData);
                case TransferMode.All:
                    return TransferAll(simulation, transaction, testData);
                case TransferMode.Pick:
                    return TransferPick(simulation, transaction, testData);
                case TransferMode.Function:
                    return TransferFunction(simulation, transaction, testData);
                case TransferMode.Parameter:
                    return TransferParameter(simulation, transaction, testData);
                case TransferMode.Subroutine:
                    return TransferSubroutine(simulation, transaction, testData);
                case TransferMode.Simultaneous:
                    return TransferSimultaneous(simulation, transaction, testData);
                default:
                    throw new NotImplementedException();

            }
        }

        private bool TransferSimultaneous(Simulation simulation, Transaction transaction, TransferTestData testData)
        {
            if (transaction.Delayed)
            {
                transaction.Delayed = false;
                transaction.NextBlock = simulation.Model.Statements.Labels[testData.SecondaryDestination];
            }
            else
                transaction.NextBlock = simulation.Model.Statements.Labels[testData.PrimaryDestination];

            return true;
        }

        // Работает иначе, чем в GPSS
        private bool TransferParameter(Simulation simulation, Transaction transaction, TransferTestData testData)
        {
            string nameBase = transaction.Parameters[testData.PrimaryDestination];
            string destination = string.Concat(nameBase, testData.SecondaryDestination);
            transaction.NextBlock = simulation.Model.Statements.Labels[destination];

            return true;
        }

        // Работает иначе, чем в GPSS
        private bool TransferSubroutine(Simulation simulation, Transaction transaction, TransferTestData testData)
        {
            var statements = simulation.Model.Statements;
            int index = statements.Transfers[this];
            string label = statements.Labels.FirstOrDefault(kvp => kvp.Value == index).Key;
            if (label == null)
                throw new ModelStructureException(
                    "Attempt to Transfer Transaction in Parameter Mode via unnamed Transfer Block.",
                    transaction.CurrentBlock);

            transaction.SetParameter(testData.SecondaryDestination, label);
            transaction.NextBlock = statements.Labels[testData.PrimaryDestination];
            return true;
        }

        // Работает иначе, чем в GPSS
        private bool TransferFunction(Simulation simulation, Transaction transaction, TransferTestData testData)
        {
            var function = simulation.Model.Calculations.Functions[testData.PrimaryDestination];
            function.Calculate(simulation.StandardAttributes);

            int value = (int)Math.Round(function.Result);
            string destination = string.Concat(testData.SecondaryDestination, value.ToString());

            transaction.NextBlock = simulation.Model.Statements.Labels[destination];
            return true;
        }

        private bool TransferPick(Simulation simulation, Transaction transaction, TransferTestData testData)
        {
            double roll = simulation.Model.Calculations.DefaultRandomGenerator.StandardUniform();
            if (roll < 0.5)
                transaction.NextBlock = simulation.Model.Statements.Labels[testData.PrimaryDestination];
            else
                transaction.NextBlock = simulation.Model.Statements.Labels[testData.SecondaryDestination];

            return true;
        }

        private bool TransferAll(Simulation simulation, Transaction transaction, TransferTestData testData)
        {
            var statements = simulation.Model.Statements;
            int primaryBlockIndex = statements.Labels[testData.PrimaryDestination];
            int secondaryBlockIndex = statements.Labels[testData.SecondaryDestination];

            if (primaryBlockIndex >= secondaryBlockIndex)
                throw new ModelStructureException(
                    "Attempt to perform tests for TRANSFER Block All Mode with " +
                    "Primary Destination Block index higher or equal to Secondary Destination Block.",
                    transaction.CurrentBlock);
            if (testData.Increment < 1)
                throw new ModelStructureException(
                    "Attempt to perform tests for TRANSFER Block All Mode with " +
                    "non-positive Increment value.",
                    transaction.CurrentBlock);
            if ((secondaryBlockIndex - primaryBlockIndex) % testData.Increment != 0)
                throw new ModelStructureException(
                    "Attempt to perform tests for TRANSFER Block All Mode with " +
                    "invalid Increment value: loop not coming into Secondary Destination Block.",
                    transaction.CurrentBlock);
            
            for (int index = primaryBlockIndex; index <= secondaryBlockIndex; index += testData.Increment)
                if (statements.Blocks[index].CanEnter(simulation))
                {
                    transaction.NextBlock = index;
                    return true;
                }

            transaction.NextBlock = transaction.CurrentBlock;
            return false;
        }

        private bool TranferBoth(Simulation simulation, Transaction transaction, TransferTestData testData)
        {
            var statements = simulation.Model.Statements;
            int primaryBlockIndex = statements.Labels[testData.PrimaryDestination];
            int secondaryBlockIndex = statements.Labels[testData.SecondaryDestination];

            if (statements.Blocks[primaryBlockIndex].CanEnter(simulation))
                transaction.NextBlock = primaryBlockIndex;
            else if (statements.Blocks[secondaryBlockIndex].CanEnter(simulation))
                transaction.NextBlock = secondaryBlockIndex;
            else
            {
                transaction.NextBlock = transaction.CurrentBlock;
                return false;
            }

            return true;
        }

        private bool TransferFractional(Simulation simulation, Transaction transaction, TransferTestData testData)
        {
            double fraction = Fraction(simulation.StandardAttributes);
            double roll = simulation.Model.Calculations.DefaultRandomGenerator.StandardUniform();
            if (roll <= fraction)
                transaction.NextBlock = simulation.Model.Statements.Labels[testData.SecondaryDestination];
            else if (testData.PrimaryDestination == null)
                transaction.NextBlock = transaction.CurrentBlock + 1;
            else
                transaction.NextBlock = simulation.Model.Statements.Labels[testData.PrimaryDestination];

            return true;
        }

        private bool TransferUnconditional(Simulation simulation, Transaction transaction, TransferTestData testData)
        {
            transaction.NextBlock = simulation.Model.Statements.Labels[testData.PrimaryDestination];
            return true;
        }

        public override Block Clone() => new Transfer
        {
            Mode = Mode,
            Fraction = Fraction,
            PrimaryDestination = PrimaryDestination,
            SecondaryDestination = SecondaryDestination,
            Increment = Increment,

            RetryChain = RetryChain.Clone(),

            EntryCount = EntryCount,
            TransactionsCount = TransactionsCount,
        };

        public bool Contains(Transaction transaction)
        {
            return RetryChain.ContainsKey(transaction);
        }

        public bool Remove(Transaction transaction)
        {
            return RetryChain.Remove(transaction);
        }
    }
}
