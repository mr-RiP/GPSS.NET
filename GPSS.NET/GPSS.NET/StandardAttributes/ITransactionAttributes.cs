namespace GPSS.StandardAttributes
{
    public interface ITransactionAttributes
    {
        /// <summary>
        /// Assembly Set of the Active Transaction. GPSS World A1 SNA.
        /// </summary>
        /// <returns>Integer index of Active Transaction Assembly Set.</returns>
        int Assembly();

        /// <summary>
        /// Match Assembly Set of the Active Transaction with Transaction at selected Block.
        /// GPSS World MB$Entnum SNA.
        /// </summary>
        /// <param name="blockNumber">Number of the Block</param>
        /// <returns>
        /// True if there is a Transaction at Block <paramref name="blockNumber"/> 
        /// which is in the same Assembly Set as the Active Transaction.
        /// False otherwise.
        /// </returns>
        bool MatchAtBlock(int blockNumber);

        /// <summary>
        /// Current absolute system clock value minus value in 
        /// Transaction Parameter <paramref name="parameterName"/>.
        /// Parameter must exist and be of numeric type.
        /// GPSS World MPParameter SNA.
        /// </summary>
        /// <param name="parameterName">Name of Active Transaction's parameter.</param>
        /// <returns>
        /// Current absolute system clock value minus value in 
        /// Transaction Parameter <paramref name="parameterName"/>.
        /// </returns>
        double TransitTime(string parameterName);

        /// <summary>
        /// Absolute system clock minus the "Mark Time" of the Transaction.
        /// GPSS World M1 SNA.
        /// </summary>
        /// <returns>Absolute system clock minus the "Mark Time" of the Transaction.</returns>
        double TransitTime();

        /// <summary>
        /// Parameter value.
        /// GPSS World PParameter SNA.
        /// </summary>
        /// <typeparam name="T">Parameter type.</typeparam>
        /// <param name="parameterName">Parameter name.</param>
        /// <returns>Parameter value.</returns>
        T Parameter<T>(string parameterName);
        /*
        double ParameterDouble(string parameterName);
        int ParameterInteger(string parameterName);
        string ParameterString(string parameterName);
        bool ParameterBoolean(string parameterName);
        */

        /// <summary>
        /// Transaction priority.
        /// GPSS World PR SNA.
        /// </summary>
        /// <returns>Transaction priority.</returns>
        int Priority();

        /// <summary>
        /// The Transaction Number of the Active Transaction.
        /// </summary>
        /// <returns>The Transaction Number of the Active Transaction.</returns>
        int ActiveTransaction();
    }
}
