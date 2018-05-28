namespace GPSS.StandardAttributes
{
    public interface IVariableAttributes<T>
    {
        /// <summary>
        /// Result of evaluating Variable Entity.
        /// </summary>
        /// <remarks>
        /// GPSS World BV$Entnum or V$Entnum SNA. Depends of the Variable type.
        /// </remarks>
        T Result { get; }
    }
}