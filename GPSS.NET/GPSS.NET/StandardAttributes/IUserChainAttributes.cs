namespace GPSS.StandardAttributes
{
    public interface IUserchainAttributes
    {
        /// <summary>
        /// Average Userchain content. The time weighted average number of chained Transactions for Userchain Entity Entnum.
        /// </summary>
        /// <remarks>
        /// GPSS World CA$Entnum SNA.
        /// </remarks>
        double AverageContent { get; }

        /// <summary>
        /// Total Userchain entries. The count of all Transactions that have been chained to the User Chain of Userchain Entity Entnum.
        /// </summary>
        /// <remarks>
        /// GPSS World CC$Entnum SNA.
        /// </remarks>
        int TotalEntries { get; }

        /// <summary>
        /// Current Userchain content. The current number of Transactions chained at Userchain Entity Entnum.
        /// </summary>
        /// <remarks>
        /// GPSS World CH$Entnum SNA.
        /// </remarks>
        int CurrentContent { get; }

        /// <summary>
        /// Maximum Userchain content. The maximum number of Transactions chained at Userchain Entity Entnum. The "high water mark".
        /// </summary>
        /// <remarks>
        /// GPSS World CM$Entnum SNA.
        /// </remarks>
        int MaximumContent { get; }

        /// <summary>
        /// Average Userchain residence time. The average duration of Transactions at Userchain Entity Entnum.
        /// </summary>
        /// <remarks>
        /// GPSS World CT$Entnum SNA.
        /// </remarks>
        double AverageResidenceTime { get; }
    }
}