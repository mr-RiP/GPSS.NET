using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GPSS.ModelParts;
using GPSS.ReportParts;
using GPSS.SimulationParts;

namespace GPSS
{
    /// <summary>
    /// Report class.
    /// </summary>
    public class Report
    {
        protected Report()
        {

        }

        internal Report(Simulation simulation)
        {
            GetCounters(simulation.Scheduler);

            GetBlocks(simulation.Model.Statements);

            GetCurrentEventsChain(simulation.Scheduler);
            GetFutureEventsChain(simulation.Scheduler);

            GetStorages(simulation.Model.Resources);
            GetFacilities(simulation.Model.Resources);

            GetSavevalues(simulation.Model.Calculations);
        }

        private void GetBlocks(Statements statements)
        {
            int count = statements.Blocks.Count;
            var list = new List<BlockData>(count);
            for (int i = 0; i < count; ++i)
                list.Add(new BlockData(statements, i));

            Blocks = list.AsReadOnly();
        }

        private void GetFutureEventsChain(TransactionScheduler scheduler)
        {
            FutureEventsChain = scheduler.FutureEvents
                .Select(fe => new FutureEventData(fe))
                .ToList().AsReadOnly();
        }

        private void GetCurrentEventsChain(TransactionScheduler scheduler)
        {
            CurrentEventsChain = scheduler.CurrentEvents
                .Select(t => new CurrentEventData(t))
                .ToList().AsReadOnly();
        }

        private void GetSavevalues(Calculations calculations)
        {
            Savevalues = calculations.Savevalues
                .Select(kvp => new SavevalueData(kvp.Key, kvp.Value))
                .ToList().AsReadOnly();
        }

        private void GetFacilities(Resources resources)
        {
            Facilities = resources.Facilities
                .Select(kvp => new FacilityData(kvp.Key, kvp.Value))
                .ToList().AsReadOnly();
        }

        private void GetStorages(Resources resources)
        {
            Storages = resources.Storages
                .Select(kvp => new StorageData(kvp.Key, kvp.Value))
                .ToList().AsReadOnly();
        }

        private void GetCounters(TransactionScheduler scheduler)
        {
            BeginTime = scheduler.BeginTime;
            EndTime = scheduler.AbsoluteTime;
            RelativeTime = scheduler.RelativeTime;
        }

        /// <summary>
        /// Simulation object's Absolute Time value at the beginning of the simulation process.
        /// </summary>
        public double BeginTime { get; private set; }
        /// <summary>
        /// Simulation object's Absolute Time value at the end of the simulation process.
        /// </summary>
        public double EndTime { get; private set; }
        /// <summary>
        /// Simulation object's Relative Time value at the end of the simulation process.
        /// </summary>
        public double RelativeTime { get; private set; }

        /// <summary>
        /// Model Blocks' simulation data collection.
        /// </summary>
        public ReadOnlyCollection<BlockData> Blocks { get; private set; }

        /// <summary>
        /// Simulation object FEC data.
        /// </summary>
        public ReadOnlyCollection<FutureEventData> FutureEventsChain { get; private set; }
        /// <summary>
        /// Simulation object CEC data.
        /// </summary>
        public ReadOnlyCollection<CurrentEventData> CurrentEventsChain { get; private set; }

        /// <summary>
        /// Model Storages' simulation data collection.
        /// </summary>
        public ReadOnlyCollection<StorageData> Storages { get; private set; }
        /// <summary>
        /// Model Facilities' simulation data collection.
        /// </summary>
        public ReadOnlyCollection<FacilityData> Facilities { get; private set; }

        /// <summary>
        /// Model Savevalues' simulation data collection.
        /// </summary>
        public ReadOnlyCollection<SavevalueData> Savevalues { get; private set; }

        #region .
        internal Report(int preset)
        {
            if (preset == 2)
                Preset1();
            else
                Preset2();
        }

        private void Preset2()
        {
            Random rand = new Random();

            BeginTime = 0.0;
            RelativeTime = 2996.661 + rand.NextDouble() * 0.0001;
            EndTime = RelativeTime;

            Blocks = new List<BlockData>
            {
                new BlockData() { EntryCount = 1000, TypeName = "GENERATE" },
                new BlockData() { EntryCount = 1110, TypeName = "GATE", Names = new List<string> { "Process" }.AsReadOnly() },
                new BlockData() { EntryCount = 1110, TypeName = "ENTER" },
                new BlockData() { EntryCount = 1110, TypeName = "SEIZE" },
                new BlockData() { EntryCount = 1110, TypeName = "LEAVE" },
                new BlockData() { EntryCount = 1110, TypeName = "ADVANCE" },
                new BlockData() { EntryCount = 1110, TypeName = "RELEASE" },
                new BlockData() { EntryCount = 1110, TypeName = "TRANSFER" },
                new BlockData() { EntryCount = 1000, TypeName = "TERMINATE", Names = new List<string> { "Finish" }.AsReadOnly() },
                new BlockData() { TypeName = "TERMINATE", Names = new List<string> { "Overload" }.AsReadOnly() },
            }.AsReadOnly();

            Facilities = new List<FacilityData>
            {
                new FacilityData()
                {
                    Name = "Processor",
                    EntryCount = 1110,
                    Utilization = 0.181 + rand.NextDouble() * 0.0001,
                    AverageHoldingTime = 0.490 + rand.NextDouble() * 0.0001,
                    Available = true,
                    OwnerNumber = null,
                }
            }.AsReadOnly();

            Storages = new List<StorageData>
            {
                new StorageData()
                {
                    Name = "ReqQueue",
                    TotalCapacity = 10,
                    OccupiedCapacity = 0,
                    MaximumCapacityUsed = 1,
                    EntryCount = 1110,
                    Available = true,
                    AverageCapacity = double.Epsilon,
                    Utilization = double.Epsilon,
                },
            }.AsReadOnly();

            CurrentEventsChain = new List<CurrentEventData>().AsReadOnly();

            FutureEventsChain = new List<FutureEventData>
            {
                new FutureEventData { Number = 1000, DepartureTime = 2998.484 + rand.NextDouble() * 0.0001, Assembly = 1000, CurrentBlockIndex = -1, NextBlockIndex = 0 },
            }.AsReadOnly();
        }

        private void Preset1()
        {
            var rand = new Random();

            BeginTime = 0.0;
            RelativeTime = 4992.918 + rand.NextDouble() * 0.0001;
            EndTime = RelativeTime;

            Blocks = new List<BlockData>
            {
                new BlockData() { EntryCount = 1004, TypeName = "GENERATE" },
                new BlockData() { EntryCount = 1004, TypeName = "GATE" },
                new BlockData() { EntryCount = 1004, TypeName = "GATE" },
                new BlockData() { EntryCount = 1004, TypeName = "ENTER" },
                new BlockData() { EntryCount = 1004, TypeName = "SEIZE" },
                new BlockData() { EntryCount = 1004, TypeName = "LEAVE" },
                new BlockData() { EntryCount = 1004, TypeName = "ADVANCE", TransactionsCount = 1 },
                new BlockData() { EntryCount = 1003, TypeName = "RELEASE" },
                new BlockData() { EntryCount = 1003, TypeName = "ENTER" },
                new BlockData() { EntryCount = 1003, TypeName = "ADVANCE", TransactionsCount = 3 },
                new BlockData() { EntryCount = 1000, TypeName = "LEAVE" },
                new BlockData() { EntryCount = 1000, TypeName = "TERMINATE" },
                new BlockData() { TypeName = "TERMINATE", Names = new List<string> { "OverloadL" }.AsReadOnly() },
            }.AsReadOnly();

            Facilities = new List<FacilityData>
            {
                new FacilityData()
                {
                    Name = "CashDesk",
                    EntryCount = 1004,
                    Utilization = 0.604 + rand.NextDouble() * 0.0001,
                    AverageHoldingTime = 3.004 + rand.NextDouble() * 0.0001,
                    Available = true,
                    OwnerNumber = 1003,
                }
            }.AsReadOnly();

            Storages = new List<StorageData>
            {
                new StorageData()
                {
                    Name = "Queue",
                    TotalCapacity = 10,
                    OccupiedCapacity = 0,
                    MaximumCapacityUsed = 1,
                    EntryCount = 1004,
                    Available = true,
                    AverageCapacity = 0.005 + rand.NextDouble() * 0.0001,
                    Utilization = 0.0001 * rand.NextDouble(),
                },
                new StorageData()
                {
                    Name = "Tables",
                    TotalCapacity = 10,
                    OccupiedCapacity = 3,
                    MaximumCapacityUsed = 6,
                    EntryCount = 1003,
                    Available = true,
                    AverageCapacity = 4.013 + rand.NextDouble() * 0.0001,
                    Utilization = 0.401 + rand.NextDouble() * 0.0001,
                }
            }.AsReadOnly();

            CurrentEventsChain = new List<CurrentEventData>().AsReadOnly();

            FutureEventsChain = new List<FutureEventData>
            {
                new FutureEventData { Number = 1004, DepartureTime = 4995.657 + rand.NextDouble() * 0.0001, Assembly = 1004, CurrentBlockIndex = -1, NextBlockIndex = 0 },
                new FutureEventData { Number = 1003, DepartureTime = 4995.725 + rand.NextDouble() * 0.0001, Assembly = 1003, CurrentBlockIndex = -1, NextBlockIndex = 0 },
                new FutureEventData { Number = 1000, DepartureTime = 4995.635 + rand.NextDouble() * 0.0001, Assembly = 1000, CurrentBlockIndex = -1, NextBlockIndex = 0 },
                new FutureEventData { Number = 1001, DepartureTime = 4995.141 + rand.NextDouble() * 0.0001, Assembly = 1001, CurrentBlockIndex = -1, NextBlockIndex = 0 },
                new FutureEventData { Number = 1002, DepartureTime = 4995.814 + rand.NextDouble() * 0.0001, Assembly = 1002, CurrentBlockIndex = -1, NextBlockIndex = 0 },
            }.AsReadOnly();
        }
        #endregion
    }
}
