namespace GPSS.StandardAttributes
{
    // TODO - Добавить генерацию случайных распределений.
    public interface IRandomNumberGeneratorAttributes
    {
        /// <summary>
        /// Random integer between 0 and 999.
        /// </summary>
        /// <remarks>
        /// GPSS World RN$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// Random integer between 0 and 999.
        /// </returns>
        int RandomInteger();

        /// <summary>
        /// Random double value between 0 and 0.99999999999999978.
        /// </summary>
        /// <returns>
        /// Pseudo random double value between 0 and 1.
        /// </returns>
        double RandomDouble();
    }
}