namespace GPSS.StandardAttributes
{
    public interface IFunctionAttributes
    {
        /// <summary>
        /// Result of evaluating Function.
        /// </summary>
        /// <remarks>
        /// GPSS World FN$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// Result of evaluating Function $Entnum.
        /// </returns>
        double Result();
    }
}