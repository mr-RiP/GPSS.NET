namespace GPSS.StandardAttributes
{
	public interface ISavevalueAttributes
	{
		/// <summary>
		/// The value of Savevalue Entity.
		/// </summary>
		/// <remarks>
		/// GPSS World X$Entnum SNA.
		/// </remarks>
		dynamic Value { get; }
	}
}