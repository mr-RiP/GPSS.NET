using GPSS.Enums;
using GPSS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPSS.Entities.General.Blocks
{
    internal class Generate : Block
    {
        private Generate()
        {

        }

        public Generate(
            Func<IStandardAttributes, double> generationInterval,
            Func<IStandardAttributes, double> firstTransactionDelay,
            Func<IStandardAttributes, int?> generationLimit,
            Func<IStandardAttributes, int> priority)
        {
            GenerationInterval = generationInterval;
            FirstTransactionDelay = firstTransactionDelay;
            GenerationLimit = generationLimit;
            Priority = priority;
        }

        public Func<IStandardAttributes, double> GenerationInterval { get; private set; }
        public Func<IStandardAttributes, double> FirstTransactionDelay { get; private set; }
        public Func<IStandardAttributes, int?> GenerationLimit { get; private set; }
        public Func<IStandardAttributes, int> Priority { get; private set; }
        public int? GenerationLimitValue { get; private set; }

        public override string TypeName => "GENERATE";

        public override Block Clone() => new Generate
        {
            GenerationInterval = GenerationInterval,
            FirstTransactionDelay = FirstTransactionDelay,
            GenerationLimit = GenerationLimit,
            Priority = Priority,
            GenerationLimitValue = GenerationLimitValue,
            EntryCount = EntryCount,
            TransactionsCount = TransactionsCount,
        };

        public override void Run(Simulation simulation)
        {
            if (simulation.ActiveTransaction.IsSet())
                throw new ModelStructureException("Active Transaction must not entry GENERATE blocks.",
                    GetBlockIndex(simulation));

            EnterBlock();
            try
            {
                bool firstGeneration = EntryCount == 0;
                if (firstGeneration)
                    GenerationLimitValue = GenerationLimit(simulation.StandardAttributes);

                if (GenerationLimitValue == null || GenerationLimitValue > EntryCount)
                {
                    Transaction transaction = GenerateTransaction(simulation, firstGeneration);
                    EnchainTransaction(simulation, transaction);
                }
            }
            catch (StandardAttributeAccessException error)
            {
                if (error.EntityType == EntityTypes.Transaction)
                    throw new ModelStructureException("Can't access Active Transaction within GENERATE block.",
                        GetBlockIndex(simulation), error);
                else
                    throw error;
            }
            ExitBlock();
        }

        private void EnchainTransaction(Simulation simulation, Transaction transaction)
        {
            if (transaction.TimeIncrement == 0.0)
                simulation.Chains.PlaceInCurrentEvents(transaction);
            else
                simulation.Chains.PlaceInFutureEvents(transaction);
        }

        private Transaction GenerateTransaction(Simulation simulation, bool firstGeneration)
        {
            int blockIndex = GetBlockIndex(simulation);
            double timeIncrement = GenerationInterval(simulation.StandardAttributes);
            if (timeIncrement < 0.0)
                throw new ModelStructureException("Negative time increment.", blockIndex);

            if (firstGeneration)
                timeIncrement += FirstTransactionDelay(simulation.StandardAttributes);
            int number = simulation.System.GenerationCount++;

            return new Transaction
            {
                Number = number,
                Assembly = number,
                MarkTime = simulation.System.AbsoluteClock,
                TimeIncrement = timeIncrement,
                CurrentBlock = blockIndex,
                NextBlock = blockIndex + 1,
                Chain = TransactionState.Passive,
                Priority = Priority(simulation.StandardAttributes),
                Trace = false,
                Preempted = false,
            };
        }

        public override void Clear()
        {
            base.Clear();
            GenerationLimitValue = null;
        }
    }
}
