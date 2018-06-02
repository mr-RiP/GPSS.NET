using GPSS.SimulationParts;
using System;
using System.Linq;

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
            ActiveTransaction = new ActiveTransaction(this);
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
            ResetSimulation(terminationCount);
            while (System.TerminationCount > 0)
            {
                if (Chains.CurrentEvents.Any())
                    RunCurrentEvents();
                else if (Chains.FutureEvents.Any())
                    UpdateEvents();
                else
                    GenerateEvents();
            }

            return new Report(this);
        }

        private void UpdateEvents()
        {
            Chains.UpdateCurrentEvents();
            System.UpdateClock(Chains.CurrentTimeIncrement());
            Chains.RefreshTimeIncrement();
        }

        private void ResetSimulation(int terminationCount)
        {
            Clear();
            System.TerminationCount = terminationCount;
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

        private void RunCurrentEvents()
        {
            while (Chains.CurrentEvents.Any())
            {
                ActiveTransaction.Reset();
                while (ActiveTransaction.IsSet())
                    ActiveTransaction.RunNextBlock();
            }
        }

        private void GenerateEvents()
        {
            Model.General.Generators.ForEach(g => g.Run(this));
        }
    }
}
