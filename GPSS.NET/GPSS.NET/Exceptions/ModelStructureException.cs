using System;

namespace GPSS.Exceptions
{
    public class ModelStructureException : Exception
    {
        public ModelStructureException(string message, int blockIndex) : base(message)
        {
            BlockIndex = blockIndex;
        }

        public ModelStructureException(string message, int blockIndex, Exception innerException) : base(message, innerException)
        {
            BlockIndex = blockIndex;
        }

        public int BlockIndex { get; private set; }
    }
}
