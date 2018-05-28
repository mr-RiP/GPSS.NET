namespace GPSS.StandardAttributes
{
    public interface INumericGroupAttributes
    {
        /// <summary>
        /// Numeric Group count.
        /// </summary>
        /// <remarks>
        /// GPSS World GN$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// Returns the membership count of Numeric Group Entity.
        /// </returns>
        int Count();
    }
}