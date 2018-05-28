namespace GPSS.StandardAttributes
{
    public interface ILogicSwitchAttributes
    {
        /// <summary>
        /// Logicswitch set.
        /// </summary>
        /// <remarks>
        /// GPSS World LS$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// Returns true if Logicswitch is in the "set" state,
        /// false otherwise.
        /// </returns>
        bool Set();
    }
}