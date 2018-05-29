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
            CloneModel(model);
            InitializeSimulationData();
        }

        internal Model Model { get; private set; }
        internal Events Events { get; private set; }
        internal StandardAttributesAccess StandardAttributes { get; private set; }

        private void InitializeSimulationData()
        {
            throw new NotImplementedException();
        }

        private void ValidateModel(Model model)
        {
            throw new NotImplementedException();
        }

        private void CloneModel(Model model)
        {
            Model = (Model)model.Clone();
        }

        public Report Start(int trasactionsCount)
        {
            throw new NotImplementedException();
        }
    }
}
