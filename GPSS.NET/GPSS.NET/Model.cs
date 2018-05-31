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

        public Model Generate(
            Func<IStandardAttributes, double> generationInterval,
            Func<IStandardAttributes, double> firstTransactionDelay,
            Func<IStandardAttributes, int?> creationLimit,
            Func<IStandardAttributes, int> priority)
        {
            if (generationInterval == null)
                throw new ArgumentNullException("generationInterval");

            var generate = new Generate(
                generationInterval,
                firstTransactionDelay ?? (sna => 0.0),
                creationLimit ?? (sna => null),
                priority ?? (sna => 0));
            return this;
        }



        #endregion

        #endregion

        #region GPSS Commands

        #endregion
    }
}
