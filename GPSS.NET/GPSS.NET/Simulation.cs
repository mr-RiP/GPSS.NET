using GPSS.Exceptions;
using GPSS.ModelParts;
using GPSS.SimulationParts;
using System;
using System.Linq;

namespace GPSS
{
    /// <summary>
    /// GPSS Simulation class.
    /// </summary>
    public class Simulation
    {
        /// <summary>
        /// Main constructor for Simulation object.
        /// </summary>
        /// <param name="model">Model to use for simulation purposes.</param>
        public Simulation(Model model)
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

        /// <summary>
        /// Simulation start.
        /// </summary>
        /// <param name="terminationCount">Termination count value must be positive.</param>
        /// <returns>Report for simulation object.</returns>
        public Report Start(int terminationCount)
        {
            InitializeStart(terminationCount);
            RunSimulation();
            CalculateResults();

            return new Report(Model.Resources.Storages.Count);
        }

        private void InitializeStart(int terminationCount)
        {
            Scheduler.TerminationCount = terminationCount;
            Scheduler.BeginTime = Scheduler.AbsoluteTime;
            run = true;            
        }

        /// <summary>
        /// Stops simulation process.
        /// </summary>
        public void Halt()
        {
            run = false;
        }

        /// <summary>
        /// Continue previously halted simulation.
        /// </summary>
        /// <returns>Report object.</returns>
        public Report Continue()    
        {
            run = true; 

            RunSimulation();
            CalculateResults();

            return new Report(Model.Resources.Storages.Count);
        }

        /// <summary>
        /// Resets model data in simulation object.
        /// </summary>
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
