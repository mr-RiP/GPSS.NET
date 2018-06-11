using GPSS.Entities.General.RetryChain;
using GPSS.Enums;
using GPSS.Exceptions;
using GPSS.Extensions;
using GPSS.SimulationParts;
using System;
using System.Collections.Generic;
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
                    var data = CalculateTestData(simulation.StandardAttributes);
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

        private TransferTestData CalculateTestData(IStandardAttributes standardAttributes)
        {
            throw new NotImplementedException();
        }

        private void Add(Transaction transaction, TransferTestData data)
        {
            throw new NotImplementedException();
        }

        private void Reset(TransactionScheduler scheduler, Transaction transaction)
        {
            throw new NotImplementedException();
        }

        private bool Resolve(Simulation simulation, Transaction transaction, TransferTestData transferTestData)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool Remove(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
