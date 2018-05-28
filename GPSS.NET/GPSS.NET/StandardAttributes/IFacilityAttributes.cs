namespace GPSS.StandardAttributes
{
    public interface IFacilityAttributes
    {
        /// <summary>
        /// Facility busy.
        /// </summary>
        /// <remarks>
        /// GPSS World F$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// If Facility is currently busy, returns true. Otherwise returns false.
        /// </returns>
        bool Busy();

        /// <summary>
        /// Facility capture count.
        /// </summary>
        /// <remarks>
        /// GPSS World FC$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// The number of times Facility $Entnum has been Seized or Prempted by a Transaction.
        /// </returns>
        int CaptureCount();

        /// <summary>
        /// Facility interrupted.
        /// </summary>
        /// <remarks>
        /// GPSS World FI$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// If Facility is currently preempted by a Transaction in an Interrupt Mode Preempt Block,
        /// returns true. Otherwise returns false.
        /// </returns>
        bool Interrupted();

        /// <summary>
        /// Facility utilization.
        /// The fraction of time Facility has been busy.
        /// </summary>
        /// <remarks>
        /// GPSS World FR$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// Facility utilization value is expressed in parts-per-thousand and therefore 
        /// returns a real value between 0 and 1000.
        /// </returns>
        double Utilization();

        /// <summary>
        /// Average facility holding time.
        /// </summary>
        /// <remarks>
        /// GPSS World FT$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// The average time Facility is owned by a capturing Transaction.
        /// </returns>
        double AverageHoldingTime();

        /// <summary>
        /// Facility in available state.
        /// </summary>
        /// <remarks>
        /// GPSS World FV$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// Returns true if Facility is in the available state, false otherwise.
        /// </returns>
        bool Available();
    }
}
