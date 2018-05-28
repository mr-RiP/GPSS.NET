namespace GPSS.StandardAttributes
{
    public interface IQueueAttributes
    {
        /// <summary>
        /// Current Queue content.
        /// The current count value of the Queue.
        /// </summary>
        /// <remarks>
        /// GPSS World Q$Entnum SNA.
        /// </remarks>
        int Content { get; }

        /// <summary>
        /// Average Queue content.
        /// The time weighted average count for the Queue.
        /// </summary>
        /// <remarks>
        /// GPSS World Q$Entnum SNA.
        /// </remarks>
        double AverageContent { get; }

        /// <summary>
        /// Maximum Queue contents.
        /// The maximum count of Queue.
        /// This is the "high water mark".
        /// </summary>
        /// <remarks>
        /// GPSS World QM$Entnum SNA.
        /// </remarks>
        int MaxContent { get; }

        /// <summary>
        /// Total Queue entries.
        /// The sum of all Queue entry counts for the Queue.
        /// </summary>
        /// <remarks>
        /// GPSS World QC$Entnum SNA.
        /// </remarks>
        int EntryCount { get; }

        /// <summary>
        /// Queue zero entry count.
        /// The number of entries of the Queue with a zero residence time.
        /// </summary>
        /// <remarks>
        /// GPSS World QZ$Entnum SNA.
        /// </remarks>
        int ZeroEntryCount { get; }

        /// <summary>
        /// Average Queue residence time.
        /// The time weighted average of the count for the Queue.
        /// </summary>
        /// <remarks>
        /// GPSS World QT$Entnum SNA.
        /// </remarks>
        double AverageResidenceTime { get; }

        /// <summary>
        /// Average Queue residence time excluding zero entries.
        /// The time weighted average of the count for the Queue
        /// not counting entries with a zero residence time.
        /// </summary>
        /// <remarks>
        /// GPSS World QX$Entnum SNA.
        /// </remarks>
        double AverageNonZeroResidenceTime { get; }
    }
}