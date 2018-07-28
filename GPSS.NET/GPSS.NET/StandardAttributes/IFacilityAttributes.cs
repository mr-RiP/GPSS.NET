namespace GPSS.StandardAttributes
{
	public interface IFacilityAttributes
	{
		/// <summary>
		/// Facility busy.
		/// If Facility is currently busy, true. Otherwise false.
		/// </summary>
		/// <remarks>
		/// GPSS World F$Entnum SNA.
		/// </remarks>
		bool Busy { get; }

		/// <summary>
		/// Facility capture count.
		/// The number of times the Facility has been Seized or Prempted by a Transaction.
		/// </summary>
		/// <remarks>
		/// GPSS World FC$Entnum SNA.
		/// </remarks>
		int CaptureCount { get; }

		/// <summary>
		/// Facility interrupted.
		/// If the Facility is currently preempted by a Transaction in an Interrupt Mode Preempt Block, true.
		/// Otherwise returns false.
		/// </summary>
		/// <remarks>
		/// GPSS World FI$Entnum SNA.
		/// </remarks>
		bool Interrupted { get; }

		/// <summary>
		/// Facility utilization.
		/// The fraction of time the Facility has been busy.
		/// Facility utilization value is expressed in parts-per-thousand and therefore 
		/// returns a real value between 0 and 1000.
		/// </summary>
		/// <remarks>
		/// GPSS World FR$Entnum SNA.
		/// </remarks>
		double Utilization();

		/// <summary>
		/// Average facility holding time.
		/// The average time the Facility is owned by a capturing Transaction.
		/// </summary>
		/// <remarks>
		/// GPSS World FT$Entnum SNA.
		/// </remarks>
		double AverageHoldingTime();

		/// <summary>
		/// Facility in available state.
		/// True if the Facility is in the available state, false otherwise.
		/// </summary>
		/// <remarks>
		/// GPSS World FV$Entnum SNA.
		/// </remarks>
		bool Available { get; }
	}
}
