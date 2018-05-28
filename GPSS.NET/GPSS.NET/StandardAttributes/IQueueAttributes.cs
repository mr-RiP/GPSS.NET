namespace GPSS.StandardAttributes
{
    public interface IQueueAttributes
    {
        /// <summary>
        /// Current Queue content.
        /// The current count value of Queue.
        /// </summary>
        /// <remarks>
        /// GPSS World Q$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// The current count value of Queue.
        /// </returns>
        int Content();

        /// <summary>
        /// Average Queue content.
        /// The time weighted average count for Queue.
        /// </summary>
        /// <remarks>
        /// GPSS World Q$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// The current count value of Queue.
        /// </returns>
        double AverageContent();

        /// <summary>
        /// Maximum Queue contents.
        /// The maximum count of Queue.
        /// This is the "high water mark".
        /// </summary>
        /// <remarks>
        /// GPSS World QM$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// The maximum count of Queue.
        /// </returns>
        int MaxContent();

        /// <summary>
        /// Total queue entries.
        /// </summary>
        /// <remarks>
        /// GPSS World QC$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// The sum of all Queue entry counts for the Queue.
        /// </returns>
        int EntryCount();

        /// <summary>
        /// Queue zero entry count.
        /// </summary>
        /// <remarks>
        /// GPSS World QZ$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// The number of entries of the Queue with a zero residence time.
        /// </returns>
        int ZeroEntryCount();

        /// <summary>
        /// Average Queue residence time.
        /// </summary>
        /// <remarks>
        /// GPSS World QT$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// The time weighted average of the count for the Queue.
        /// </returns>
        double AverageResidenceTime();

        /// <summary>
        /// Average Queue residence time excluding zero entries.
        /// </summary>
        /// <remarks>
        /// GPSS World QX$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// The time weighted average of the count for the Queue
        /// not counting entries with a zero residence time.
        /// </returns>
        double AverageNonZeroResidenceTime();
    }
}