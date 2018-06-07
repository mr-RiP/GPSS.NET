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

        public int GenerationCount { get; private set; } = 0;

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
            GenerationCount = GenerationCount,
        };

        public void GenerateTransaction(Simulation simulation)
        {
            try
            {
                bool firstGeneration = EntryCount == 0;
                if (firstGeneration)
                    GenerationLimitValue = GenerationLimit(simulation.StandardAttributes);

                if (GenerationLimitValue == null || GenerationLimitValue > GenerationCount)
                {
                    int number = simulation.Scheduler.GenerationCount;
                    double timeIncrement = GenerationInterval(simulation.StandardAttributes);
                    if (timeIncrement < 0.0)
                        throw new ModelStructureException(
                            "Negative time increment.", 
                            simulation.Model.Statements.Generators[this]);

                    if (firstGeneration)
                        timeIncrement += FirstTransactionDelay(simulation.StandardAttributes);

                    var transaction = new Transaction
                    {
                        Number = number,
                        Assembly = number,
                        MarkTime = simulation.Scheduler.AbsoluteClock + timeIncrement,
                        CurrentBlock = -1,
                        NextBlock = simulation.Model.Statements.Generators[this],
                        State = TransactionState.Suspended,
                        Priority = Priority(simulation.StandardAttributes),
                    };

                    if (timeIncrement == 0.0)
                        simulation.Scheduler.PlaceInCurrentEvents(transaction);
                    else
                        simulation.Scheduler.PlaceInFutureEvents(
                            transaction, simulation.Scheduler.RelativeClock + timeIncrement);

                    GenerationCount++;
                    simulation.Scheduler.GenerationCount++;
                }
            }
            catch (StandardAttributeAccessException error)
            {
                if (error.EntityType == EntityTypes.Transaction)
                    throw new ModelStructureException(
                        "Can't access Active Transaction within GENERATE block.",
                        simulation.Model.Statements.Generators[this], error);
                else
                    throw error;
            }
        }

        public override void EnterBlock(Simulation simulation)
        {
            if (simulation.ActiveTransaction.Transaction.CurrentBlock < 0)
                base.EnterBlock(simulation);
            else
                throw new ModelStructureException(
                    "Invalid attempt to enter GENERATE Block.", 
                    simulation.Model.Statements.Generators[this]);
        }
    }
}
