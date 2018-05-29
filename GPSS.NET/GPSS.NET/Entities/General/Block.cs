using GPSS.StandardAttributes;
using System;

namespace GPSS.Entities.General
{
    internal abstract class Block : ICloneable, IBlockAttributes
    {
        abstract public string TypeName { get; }

        public int EntryCount { get; protected set; }

        public int TransactionsCount { get; protected set; }

        abstract public Block Clone();
        object ICloneable.Clone() => Clone();

        abstract public void Run(Simulation s);
    }
}
