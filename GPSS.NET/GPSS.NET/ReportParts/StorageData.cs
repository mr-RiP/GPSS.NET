using GPSS.Entities.Resources;

namespace GPSS.ReportParts
{
    /// <summary>
    /// Storage Entity simulation data class.
    /// </summary>
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

        /// <summary>
        /// Storage Entity name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Storage total capacity.
        /// </summary>
        public int TotalCapacity { get; private set; }

        /// <summary>
        /// Storage currently occupied capacity.
        /// </summary>
        public int OccupiedCapacity { get; private set; }

        /// <summary>
        /// Storage's transactions entry count.
        /// </summary>
        public int EntryCount { get; private set; }

        /// <summary>
        /// Storage Available flag.
        /// </summary>
        public bool Available { get; private set; }

        /// <summary>
        /// Storage time-weighted average capacity.
        /// </summary>
        public double AverageCapacity { get; private set; }

        /// <summary>
        /// Storage average holding time.
        /// </summary>
        public double AverageHoldingTime { get; private set; }

        /// <summary>
        /// Storage maximum capacity used. The "high water mark".
        /// </summary>
        public int MaximumCapacityUsed { get; private set; }

        /// <summary>
        /// Storage utilization value.
        /// </summary>
        public double Utilization { get; private set; }

        /// <summary>
        /// Storage's Delay Chain content count.
        /// </summary>
        public int DelayChainCount { get; private set; }

        /// <summary>
        /// Storage's Retry Chain content count.
        /// </summary>
        public int RetryChainCount { get; private set; }
    }
}
