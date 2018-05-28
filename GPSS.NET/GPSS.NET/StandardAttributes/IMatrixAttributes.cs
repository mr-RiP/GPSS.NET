namespace GPSS.StandardAttributes
{
    public interface IMatrixAttributes
    {
        /// <summary>
        /// Matrix Savevalue.
        /// </summary>
        /// <remarks>
        /// GPSS World MX$Entnum(m,n) SNA.
        /// </remarks>
        /// <param name="row">
        /// Matrix row index.
        /// </param>
        /// <param name="column">
        /// Matrix column index.
        /// </param>
        /// <returns>
        /// Access to Attributes of Savevalue stored in Matrix.
        /// </returns>
        ISavevalueAttributes Savevalue(int row, int column);
    }
}