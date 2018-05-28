namespace GPSS.StandardAttributes
{
    public interface ITransactionGroupAttributes
    {
        /// <summary>
        /// The membership count of the Transaction Group.
        /// </summary>
        /// <remarks>
        /// GPSS World GT$Entnum SNA.
        /// </remarks>
        int Count { get; }
    }
}