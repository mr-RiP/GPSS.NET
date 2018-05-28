using GPSS.ModelParts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS
{
    public class Model
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

        internal General General { get; } = new General();
        internal Calculations Calculations { get; } = new Calculations();
        internal Groups Groups { get; } = new Groups();
        internal Resources Resources { get; } = new Resources();
        internal Statistics Statistics { get; } = new Statistics();

        #region GPSS Blocks

        #endregion

        #region GPSS Commands

        #endregion
    }
}
