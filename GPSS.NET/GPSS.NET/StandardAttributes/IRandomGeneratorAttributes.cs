namespace GPSS.StandardAttributes
{
    // TODO - Добавить генерацию случайных распределений.
    public interface IRandomGeneratorAttributes
    {
        /// <summary>
        /// Random number between 0 and 999.
        /// </summary>
        /// <remarks>
        /// GPSS World RN$Entnum SNA.
        /// </remarks>
        /// <returns>
        /// A random integer 0-999 from the Random Number Generator.
        /// </returns>
        int RandomNumber();
    }
}