namespace GPSS.StandardAttributes
{
    public interface IBlockAttributes
    {
        /// <summary>
        /// Block entry count.
        /// </summary>
        /// <remarks>
        /// GPSS World N$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// The total number of Transactions which have entered Block.
        /// </returns>
        int EntryCount();

        /// <summary>
        /// Block transactions count.
        /// </summary>
        /// <remarks>
        /// GPSS World W$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// The current number of Transactions in Block.
        /// </returns>
        int TransactionsCount();
    }
}