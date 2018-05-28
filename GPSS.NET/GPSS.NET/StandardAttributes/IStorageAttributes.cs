namespace GPSS.StandardAttributes
{
    public interface IStorageAttributes
    {
        /// <summary>
        /// Unused storage capacity.
        /// The storage content (or spaces available for use by "tokens") 
        /// available for use by entering Transactions at the Storage Entity.
        /// </summary>
        /// <remarks>
        /// GPSS World R$Entnum SNA.
        /// </remarks>
        int AvailableCapacity { get; }

        /// <summary>
        /// Storage in use.
        /// The amount of storage content (or "token" spaces) currently in use 
        /// by entering Transactions at the Storage Entity.
        /// </summary>
        /// <remarks>
        /// GPSS World S$Entnum SNA.
        /// </remarks>
        int OccupiedCapacity { get; }

        /// <summary>
        /// Average storage in use.
        /// The time weighted average of storage capacity (or "token" spaces)
        /// in use at the Storage Entity.
        /// </summary>
        /// <remarks>
        /// GPSS World SA$Entnum SNA.
        /// </remarks>
        double AverageCapacity { get; }

        /// <summary>
        /// Storage use count.
        /// Total number of storage units that have been acquired from the Storage Entity.
        /// </summary>
        /// <remarks>
        /// GPSS World SC$Entnum SNA.
        /// </remarks>
        int UseCount { get; }

        /// <summary>
        /// Storage empty.
        /// True if Storage the Entity is completely unused, false otherwise.
        /// </summary>
        /// <remarks>
        /// GPSS World SE$Entnum SNA.
        /// </remarks>
        bool Empty { get; }

        /// <summary>
        /// Storage full.
        /// True if the Storage Entity is completely used, false otherwise.
        /// </summary>
        /// <remarks>
        /// GPSS World SF$Entnum SNA.
        /// </remarks>
        bool Full { get; }

        /// <summary>
        /// Storage utilization.
        /// The fraction of total usage represented by 
        /// the average storage in use at the Storage Entity.
        /// Storage utilization is expressed in parts-per-thousand 
        /// and therefore returns a real value between 0 and 1000, inclusively.
        /// </summary>
        /// <remarks>
        /// GPSS World SR$Entnum SNA.
        /// </remarks>
        double Utilization { get; }

        /// <summary>
        /// Maximum storage in use at the Storage Entity. 
        /// The "high water mark".
        /// </summary>
        /// <remarks>
        /// GPSS World SM$Entnum SNA.
        /// </remarks>
        int MaximumCapacityUsed { get; }

        /// <summary>
        /// Average holding time per unit at the Storage Entity.
        /// </summary>
        /// <remarks>
        /// GPSS World ST$Entnum SNA.
        /// </remarks>
        double AverageHoldingTime { get; }

        /// <summary>
        /// Storage Entity in available state.
        /// True if the Storage Entity is in the available state, false otherwise.
        /// </summary>
        /// <remarks>
        /// GPSS World SV$Entnum SNA.
        /// </remarks>
        bool Available { get; }
    }
}