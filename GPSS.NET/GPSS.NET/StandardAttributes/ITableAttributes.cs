namespace GPSS.StandardAttributes
{
	public interface ITableAttributes
	{
		/// <summary>
		/// Nonweighted average of entries in the Table Entity.
		/// </summary>
		/// <remarks>
		/// GPSS World TB$Entnum SNA.
		/// </remarks>
		double Average { get; }

		/// <summary>
		/// Count of nonweighted table entries in the Table Entity.
		/// </summary>
		/// <remarks>
		/// GPSS World TC$Entnum SNA.
		/// </remarks>
		int Count { get; }

		/// <summary>
		/// Standard deviation of nonweighted table entries in the Table Entity.
		/// </summary>
		/// <remarks>
		/// GPSS World TD$Entnum SNA.
		/// </remarks>
		double StandardDeviation { get; }
	}
}