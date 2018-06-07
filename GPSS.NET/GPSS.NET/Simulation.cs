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
        internal TransactionScheduler Scheduler { get; private set; }

        // Аксессоры
        internal ActiveTransaction ActiveTransaction { get; private set; }
        internal StandardAttributesAccess StandardAttributes { get; private set; }

        // Флаг останова для команд Halt() и Report()
        private bool run = true;

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
            InitializeSimulation(terminationCount);
            while (Scheduler.TerminationCount > 0 && run)
            {
                if (Scheduler.CurrentEvents.Any())
                    RunCurrentEvents();
                else if (Scheduler.FutureEvents.Any())
                    UpdateEvents();
                else
                    GenerateEvents();
            }
            CalculateResultStats();

            return new Report(this);
        }

        // Вычисление итоговой статистики
        private void CalculateResultStats()
        {
            Model.Resources.Calculate(Scheduler);
        }

        private void UpdateEvents()
        {
            Scheduler.UpdateEvents();
        }

        private void InitializeSimulation(int terminationCount)
        {
            Model = initialModel.Clone();
            Scheduler = new TransactionScheduler(Model, terminationCount);

            run = true;
        }

        private void RunCurrentEvents()
        {
            while (Scheduler.CurrentEvents.Any())
            {
                ActiveTransaction.Reset();
                while (ActiveTransaction.IsSet())
                    ActiveTransaction.RunNextBlock();
            }
        }

        private void GenerateEvents()
        {
            foreach (var generator in Model.Statements.Generators.Keys)
                generator.GenerateTransaction(this);
        }
    }
}
