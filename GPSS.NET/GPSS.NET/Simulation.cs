using GPSS.ModelParts;
using GPSS.SimulationParts;
using System;

namespace GPSS
{
    public class Simulation
    {
        internal Simulation(Model model)
        {
            ValidateModel(model);
            CopyModelData(model);
            InitializeSimulationData();
        }

        internal General General { get; private set; }
        internal Calculations Calculations { get; private set; }
        internal Groups Groups { get; private set; }
        internal Resources Resources { get; private set; }
        internal Statistics Statistics { get; private set; }

        internal Clock Clock { get; private set; }
        internal Counters Counters { get; private set; }
        internal StandardAttributesAccess StandardAttributes { get; private set; }

        private void InitializeSimulationData()
        {
            throw new NotImplementedException();
        }

        private void ValidateModel(Model model)
        {
            throw new NotImplementedException();
        }

        private void CopyModelData(Model model)
        {
            throw new NotImplementedException();
        }

        public Report Start(int trasactionsCount)
        {
            throw new NotImplementedException();
        }
    }
}
