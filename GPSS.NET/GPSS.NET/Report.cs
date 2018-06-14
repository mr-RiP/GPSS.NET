using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GPSS.ModelParts;
using GPSS.ReportParts;
using GPSS.SimulationParts;

namespace GPSS
{
    public class Report
    {
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

        public double BeginTime { get; private set; }
        public double EndTime { get; private set; }
        public double RelativeTime { get; private set; }

        public ReadOnlyCollection<BlockData> Blocks { get; private set; }

        public ReadOnlyCollection<FutureEventData> FutureEventsChain { get; private set; }
        public ReadOnlyCollection<CurrentEventData> CurrentEventsChain { get; private set; }

        public ReadOnlyCollection<StorageData> Storages { get; private set; }
        public ReadOnlyCollection<FacilityData> Facilities { get; private set; }

        public ReadOnlyCollection<SavevalueData> Savevalues { get; private set; }
    }
}
