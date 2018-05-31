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
        internal TransactionChains Chains { get; private set; }
        internal SystemCounters System { get; private set; }
        internal ActiveTransaction ActiveTransaction { get; private set; }
        internal StandardAttributesAccess StandardAttributes { get; private set; }

        private void InitializeSimulationData()
        {
            Chains = new TransactionChains();
            System = new SystemCounters();
            ActiveTransaction = new ActiveTransaction();
            StandardAttributes = new StandardAttributesAccess(this);
        }

        private void ValidateModel(Model model)
        {
            throw new NotImplementedException();
        }

        private void CloneModel(Model model)
        {
            Model = model.Clone();
        }

        public Report Start(int terminationCount)
        {
            Clear();
            System.TerminationCount = terminationCount;
            while (System.TerminationCount > 0)
            {
                if (ActiveTransaction.Set(Chains))
                {
                    UpdateTime();
                    RunModel();
                }
                else
                    GenerateTransactions();
            }

            return new Report(this);
        }

        public void Clear()
        {
            Model.General.Clear();
            Model.Calculations.Clear();
            Model.Groups.Clear();
            Model.Resources.Clear();
            Model.Statistics.Clear();
            Chains.Clear();
            System.Clear();
        }

        private void UpdateTime()
        {
            System.UpdateClock(ActiveTransaction.Transaction);
        }

        private void RunModel()
        {
            while (ActiveTransaction.Set())
                ActiveTransaction.RunNextBlock(this);
        }

        private void GenerateTransactions()
        {
            Model.General.Generators.ForEach(g => g.Run(this));
        }
    }
}
