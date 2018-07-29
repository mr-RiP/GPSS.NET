namespace GPSS.StandardAttributes
{
    public interface IFunctionAttributes
    {
        /// <summary>
        /// Result of evaluating the Function.
        /// </summary>
        /// <remarks>
        /// GPSS World FN$Entnum SNA.
        /// </remarks>
        double Result { get; }
    }
}