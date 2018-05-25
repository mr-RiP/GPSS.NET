namespace GPSS.Attributes
{
    public interface IBlockAttributes
    {
        /// <summary>
        /// Block entry count.
        /// GPSS World NEntnum SNA.
        /// </summary>
        /// <param name="blockNumber">Number of the Block.</param>
        /// <returns>
        /// The total number of Transactions which have entered Block <paramref name="blockNumber"/>.
        /// </returns>
        int EntryCount(int blockNumber);

        // WEntnum - Current Block count.The current number of Transactions in Block Entnum is returned.
        /// <summary>
        /// Block transactions count.
        /// GPSS World WEntnum SNA.
        /// </summary>
        /// <param name="blockNumber">Number of the Block.</param>
        /// <returns>
        /// The current number of Transactions in Block <paramref name="blockNumber"/>.
        /// </returns>
        int TransactionsCount(int blockNumber);
    }
}