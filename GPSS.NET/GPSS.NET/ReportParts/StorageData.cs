using GPSS.Entities.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.ReportParts
{
    public class StorageData
    {
        internal StorageData(string name, Storage storage)
        {
            Name = name;
            TotalCapacity = storage.Capacity;
            OccupiedCapacity = storage.OccupiedCapacity;
            EntryCount = storage.EntryCount;
            Available = storage.Available;
            AverageCapacity = storage.AverageCapacity();
            AverageHoldingTime = storage.AverageHoldingTime();
            MaximumCapacityUsed = storage.MaximumCapacityUsed();
            Utilization = storage.Utilization();
            DelayChainCount = storage.DelayChain.Count;
            RetryChainCount = storage.RetryChain.Count;
        }

        public string Name { get; private set; }

        public int TotalCapacity { get; private set; }

        public int OccupiedCapacity { get; private set; }

        public int EntryCount { get; private set; }

        public bool Available { get; private set; }

        public double AverageCapacity { get; private set; }

        public double AverageHoldingTime { get; private set; }

        public int MaximumCapacityUsed { get; private set; }

        public double Utilization { get; private set; }

        public int DelayChainCount { get; private set; }

        public int RetryChainCount { get; private set; }
    }
}
