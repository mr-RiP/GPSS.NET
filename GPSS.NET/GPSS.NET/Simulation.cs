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
            CloneInitialModel(model);
            InitializeAccessors();
        }

        // Клон изначальной модели, используется как прототип при дальнейшем моделировании
        private Model initialModel;

        // Данные моделирования - не существуют до старта симуляции (или хранят данные прошлой симуляции)
        internal Model Model { get; private set; }
        internal TransactionChains Chains { get; private set; }
        internal SystemCounters System { get; private set; }

        // Аксессоры
        internal ActiveTransaction ActiveTransaction { get; private set; }
        internal StandardAttributesAccess StandardAttributes { get; private set; }

        private void InitializeAccessors()
        {
            ActiveTransaction = new ActiveTransaction(this);
            StandardAttributes = new StandardAttributesAccess(this);
        }

        private static void ValidateModel(Model model)
        {
            throw new NotImplementedException();
        }

        private void CloneInitialModel(Model model)
        {
            initialModel = model.Clone();
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
            Model = initialModel.Clone();
            Chains = new TransactionChains();
            System = new SystemCounters();

            System.TerminationCount = terminationCount;
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
            Model.Statements.Generators.ForEach(g => g.Run(this));
        }
    }
}
