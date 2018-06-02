using GPSS.Entities.General.Blocks;
using GPSS.ModelParts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS
{
    public class Model : ICloneable
    {
        private Model()
        {            
        }

        public static Model CreateModel()
        {
            return new Model();
        }

        public Simulation CreateSimulation()
        {
            return new Simulation(this);
        }

        internal General General { get; private set; } = new General();
        internal Calculations Calculations { get; private set; } = new Calculations();
        internal Groups Groups { get; private set; } = new Groups();
        internal Resources Resources { get; private set; } = new Resources();
        internal Statistics Statistics { get; private set; } = new Statistics();

        public Model Clone() => new Model()
        {
            General = General.Clone(),
            Calculations = Calculations.Clone(),
            Groups = Groups.Clone(),
            Resources = Resources.Clone(),
            Statistics = Statistics.Clone(),
        };

        object ICloneable.Clone() => Clone();

        #region GPSS Blocks

        #region Generate Block

        /// <summary>
        /// GENERATE Block. 
        /// A GENERATE Block creates Transactions for future entry into the simulation.
        /// </summary>
        /// <param name="generationInterval">Transaction creation interval, in model time. Zero if null.</param>
        /// <param name="firstTransactionDelay">First transaction creation delay, in model time. Zero if null.</param>
        /// <param name="creationLimit">Transactions creation limit. Null means no limit.</param>
        /// <param name="priority">Created transaction's priority value. Zero if null.</param>
        /// <returns>Model with added GENERATE Block.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="generationInterval"/> or <paramref name="creationLimit"/> must not be null.
        /// </exception>
        public Model Generate(
            Func<IStandardAttributes, double> generationInterval,
            Func<IStandardAttributes, double> firstTransactionDelay = null,
            Func<IStandardAttributes, int?> creationLimit = null,
            Func<IStandardAttributes, int> priority = null)
        {
            if (generationInterval == null && creationLimit == null)
                throw new ArgumentNullException();

            var generate = new Generate(
                generationInterval ?? (sna => 0.0),
                firstTransactionDelay ?? (sna => 0.0),
                creationLimit ?? (sna => null),
                priority ?? (sna => 0));

            General.Blocks.Add(generate);
            return this;
        }

        /// <summary>
        /// ADVANCE Block.
        /// An ADVANCE Block delays the progress of a Transaction for a specified amount of simulated time.
        /// </summary>
        /// <param name="delay">Transaction advancement delay, in model time. Zero if null.</param>
        /// <returns>Model with added ADVANCE Block.</returns>
        public Model Advance(Func<IStandardAttributes, double> delayTime)
        {
            var advance = new Advance(delayTime ?? (sna => 0.0));
            General.Blocks.Add(advance);
            return this;
        }

        /// <summary>
        /// TERMINATE Block.
        /// A TERMINATE Block removes the Active Transaction from the simulation and optionally reduces the Termination Count.
        /// </summary>
        /// <param name="terminationDecrement">Termination Count decrement. Zero if null.</param>
        /// <returns>Model with added TERMINATE Block.</returns>
        public Model Terminate(Func<IStandardAttributes, int> terminationDecrement)
        {
            var terminate = new Terminate(terminationDecrement ?? (sna => 0));
            General.Blocks.Add(terminate);
            return this;
        }

        #endregion

        #endregion

        #region GPSS Commands

        #endregion
    }
}
