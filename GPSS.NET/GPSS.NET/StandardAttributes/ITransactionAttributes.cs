namespace GPSS.StandardAttributes
{
    public interface ITransactionAttributes
    {
        /// <summary>
        /// Assembly Set Number of the Active Transaction.
        /// </summary>
        /// <remarks>
        /// GPSS World A1 SNA.
        /// </remarks>
        int Assembly { get; }

        /// <summary>
        /// Match Assembly Set of the Active Transaction with Transaction at selected Block.
        /// </summary>
        /// <remarks>
        /// GPSS World MB$Entnum SNA.
        /// </remarks>
        /// <param name="blockName">Number of the Block</param>
        /// <returns>
        /// True if there is a Transaction at Block <paramref name="blockName"/> 
        /// which is in the same Assembly Set as the Active Transaction.
        /// False otherwise.
        /// </returns>
        bool MatchAtBlock(string blockName);

        /// <summary>
        /// Current absolute system clock value minus value in 
        /// Transaction Parameter <paramref name="parameterName"/>.
        /// Parameter must exist and be of numeric type.
        /// </summary>
        /// <remarks>
        /// GPSS World MP*Parameter SNA.
        /// </remarks>
        /// <param name="parameterName">Name of Active Transaction's parameter.</param>
        /// <returns>
        /// Current absolute system clock value minus value in 
        /// Transaction Parameter <paramref name="parameterName"/>.
        /// </returns>
        double TransitTime(string parameterName);

        /// <summary>
        /// Absolute system clock minus the "Mark Time" of the Transaction.
        /// </summary>
        /// <remarks>
        /// GPSS World M1 SNA.
        /// </remarks>
        double TransitTime();

        /// <summary>
        /// Parameter value.
        /// </summary>
        /// <remarks>
        /// GPSS World P*Parameter SNA.
        /// </remarks>
        /// <param name="parameterName">Parameter name.</param>
        dynamic Parameter(string parameterName);

        /// <summary>
        /// Transaction priority.
        /// </summary>
        /// <remarks>
        /// GPSS World PR SNA.
        /// </remarks>
        int Priority { get; }

        /// <summary>
        /// The Transaction Number of the Active Transaction.
        /// </summary>
        /// <remarks>
        /// GPSS World XN1 SNA.
        /// </remarks>
        int Number { get; }
    }
}
