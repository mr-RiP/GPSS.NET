using GPSS.Entities.Calculations;
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

        internal Statements Statements { get; private set; } = new Statements();
        internal Calculations Calculations { get; private set; } = new Calculations();
        internal Groups Groups { get; private set; } = new Groups();
        internal Resources Resources { get; private set; } = new Resources();
        internal Statistics Statistics { get; private set; } = new Statistics();

        public Model Clone() => new Model()
        {
            Statements = Statements.Clone(),
            Calculations = Calculations.Clone(),
            Groups = Groups.Clone(),
            Resources = Resources.Clone(),
            Statistics = Statistics.Clone(),
        };

        object ICloneable.Clone() => Clone();

        #region GPSS Blocks

        #region GENERATE Block // TODO

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

            Statements.Blocks.Add(generate);
            return this;
        }

        #endregion

        #region ADVANCE Block

        /// <summary>
        /// ADVANCE Block.
        /// An ADVANCE Block delays the progress of a Transaction for a specified amount of simulated time.
        /// </summary>
        /// <param name="delayTime">
        /// Transaction advancement delay, in model time. Must have zero or positive value.
        /// </param>
        /// <returns>Model with added ADVANCE Block.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <param name="delayTime"> must have zero or positive value.
        /// </exception>
        public Model Advance(double delayTime)
        {
            if (delayTime < 0.0)
                throw new ArgumentOutOfRangeException(
                    nameof(delayTime),
                    "Argument must have zero or positive value.");

            return Advance((sna => delayTime));
        }

        /// <summary>
        /// ADVANCE Block.
        /// An ADVANCE Block delays the progress of a Transaction for a specified amount of simulated time.
        /// </summary>
        /// <param name="delayMean">
        /// Transaction advancement delay mean, in model time. Must have zero or positive value.
        /// </param>
        /// <param name="delayHalfRange">
        /// Transaction advancement delay half-range, in model time. Must be equal or lower than <paramref name="delayMean"/>.
        /// </param>
        /// <returns>Model with added ADVANCE Block.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <param name="delayMean"> must have zero or positive value.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="delayHalfRange"/> value must be lower or equal to <paramref name="delayMean"/> value.
        /// </exception>
        public Model Advance(double delayMean, double delayHalfRange)
        {
            if (delayMean < 0.0)
                throw new ArgumentOutOfRangeException(nameof(delayMean), "Mean value must be zero or positive.");
            if (delayMean < delayHalfRange)
                throw new ArgumentException(
                    nameof(delayHalfRange) + " value must be lower or equal to " + nameof(delayMean) + " value.");

            double min = delayMean - delayHalfRange;
            double max = delayMean + delayHalfRange;
            return Advance((sna => sna.RandomNumberGenerator().ProbabilityDistributions.Uniform(min, max)));
        }

        /// <summary>
        /// ADVANCE Block.
        /// An ADVANCE Block delays the progress of a Transaction for a specified amount of simulated time.
        /// </summary>
        /// <param name="delayTime">Transaction advancement delay, in model time. Zero if null.</param>
        /// <returns>Model with added ADVANCE Block.</returns>
        public virtual Model Advance(Func<IStandardAttributes, double> delayTime)
        {
            var advance = new Advance(delayTime ?? (sna => 0.0));
            Statements.Blocks.Add(advance);
            return this;
        }

        #endregion

        #region TERMINATE Block

        /// <summary>
        /// TERMINATE Block.
        /// A TERMINATE Block removes the Active Transaction from the simulation and optionally reduces the Termination Count.
        /// </summary>
        /// <param name="terminationDecrement">Termination Count decrement. Must be zero or positive.</param>
        /// <returns>Model with added TERMINATE Block.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="terminationDecrement"/> must have zero or positive value.
        /// </exception>
        public Model Terminate(int terminationDecrement)
        {
            if (terminationDecrement < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(terminationDecrement),
                    "Argument must have zero or positive value.");

            return Terminate((sna => terminationDecrement));
        }

        /// <summary>
        /// TERMINATE Block.
        /// A TERMINATE Block removes the Active Transaction from the simulation and optionally reduces the Termination Count.
        /// </summary>
        /// <param name="terminationDecrement">Termination Count decrement. Zero if null.</param>
        /// <returns>Model with added TERMINATE Block.</returns>
        public virtual Model Terminate(Func<IStandardAttributes, int> terminationDecrement)
        {
            var terminate = new Terminate(terminationDecrement ?? (sna => 0));
            Statements.Blocks.Add(terminate);
            return this;
        }

        #endregion

        #endregion

        #region GPSS Commands

        /// <summary>
        /// VARIABLE Command.
        /// A VARIABLE Command defines an arithmetic Variable Entity.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="expression">Variable expression.</param>
        /// <returns>Model with added VARIABLE Entity.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> and <paramref name="expression"/> must not be null.
        /// </exception>
        public Model Variable(string name, Func<IStandardAttributes, int> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(name));

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (Calculations.Variables.ContainsKey(name))
                Calculations.Variables[name].Expression = expression;
            else
                Calculations.Variables.Add(name, new Variable<int>(expression));

            return this;
        }

        /// <summary>
        /// BVARIABLE Command.
        /// A BVARIABLE Command defines a Boolean Variable Entity.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="expression">Variable expression.</param>
        /// <returns>Model with added BVARIABLE Entity.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> and <paramref name="expression"/> must not be null.
        /// </exception>
        public Model BoolVariable(string name, Func<IStandardAttributes, bool> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(name));

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (Calculations.BoolVariables.ContainsKey(name))
                Calculations.BoolVariables[name].Expression = expression;
            else
                Calculations.BoolVariables.Add(name, new Variable<bool>(expression));

            return this;
        }

        /// <summary>
        /// FVARIABLE Command.
        /// A FVARIABLE Command defines a Floating point Variable Entity.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="expression">Variable expression.</param>
        /// <returns>Model with added BVARIABLE Entity.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> and <paramref name="expression"/> must not be null.
        /// </exception>
        public Model FloatVariable(string name, Func<IStandardAttributes, double> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(name));

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (Calculations.FloatVariables.ContainsKey(name))
                Calculations.FloatVariables[name].Expression = expression;
            else            
                Calculations.FloatVariables.Add(name, new Variable<double>(expression));

            return this;
        }

        #endregion
    }
}
