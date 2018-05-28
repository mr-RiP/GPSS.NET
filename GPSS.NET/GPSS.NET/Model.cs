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

        public object Clone()
        {
            return new Model()
            {
                General = (General)General.Clone(),
                Calculations = (Calculations)Calculations.Clone(),
                Groups = (Groups)Groups.Clone(),
                Resources = (Resources)Resources.Clone(),
                Statistics = (Statistics)Statistics.Clone(),
            };
        }

        internal General General { get; private set; } = new General();
        internal Calculations Calculations { get; private set; } = new Calculations();
        internal Groups Groups { get; private set; } = new Groups();
        internal Resources Resources { get; private set; } = new Resources();
        internal Statistics Statistics { get; private set; } = new Statistics();

        #region GPSS Blocks

        #endregion

        #region GPSS Commands

        #endregion
    }
}
