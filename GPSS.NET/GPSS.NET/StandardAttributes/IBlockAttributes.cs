namespace GPSS.StandardAttributes
{
	public interface IBlockAttributes
	{
		/// <summary>
		/// Block entry count. The total number of Transactions which have entered the Block.
		/// </summary>
		/// <remarks>
		/// GPSS World N$Entnum SNA.
		/// </remarks>
		int EntryCount { get; }

		/// <summary>
		/// Block transactions count. The current number of Transactions in the Block.
		/// </summary>
		/// <remarks>
		/// GPSS World W$Entnum SNA.
		/// </remarks>
		int TransactionsCount { get; }
	}
}