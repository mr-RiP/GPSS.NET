using GPSS.Exceptions;
using GPSS.ModelParts;
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
            SetupSimulation();
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

        private void SetupSimulation()
        {
            Model = initialModel.Clone();
            Scheduler = new TransactionScheduler(Model, 0);
        }

        private static void ValidateModel(Model model)
        {
            ValidateLabels(model.Statements);
        }

        private static void ValidateLabels(Statements statements)
        {
            var badLabel = statements.Labels
                .Any(kvp => kvp.Value >= statements.Blocks.Count);

            if (badLabel)
                throw new ModelStructureException(
                    "Label naming non-existing Block found.",
                    statements.Blocks.Count);

        }

        private void CloneInitialModel(Model model)
        {
            initialModel = model.Clone();
        }

        public Report Start(int terminationCount)
        {
            InitializeStart(terminationCount);
            RunSimulation();
            CalculateResults();

            return new Report(this);
        }

        private void InitializeStart(int terminationCount)
        {
            Scheduler.TerminationCount = terminationCount;
            Scheduler.BeginTime = Scheduler.AbsoluteTime;
            run = true;            
        }

        public void Halt()
        {
            run = false;
        }

        public Report Continue()    
        {
            run = true; 

            RunSimulation();
            CalculateResults();

            return new Report(this);
        }

        public void Clear()
        {
            run = false;
            SetupSimulation();
        }

        private void RunSimulation()
        {
            while (Scheduler.TerminationCount > 0 && run)
            {
                if (Scheduler.CurrentEvents.Any())
                    RunCurrentEvents();
                else if (Scheduler.FutureEvents.Any())
                    UpdateEvents();
                else
                    GenerateEvents();
            }
        }

        // Вычисление итоговой статистики
        private void CalculateResults()
        {
            Model.Resources.Calculate(Scheduler);
        }

        private void UpdateEvents()
        {
            Scheduler.UpdateEvents();
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
