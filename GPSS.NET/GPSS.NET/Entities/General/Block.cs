using GPSS.Extensions;
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

        public virtual void EnterBlock(Simulation simulation)
        {
            EntryCount++;
            TransactionsCount++;
            var transaction = simulation.ActiveTransaction.Transaction;
            transaction.CurrentBlock = transaction.NextBlock;
            transaction.NextBlock++;
        }

        public virtual void ExitBlock(Simulation simulation)
        {
            TransactionsCount--;
        }
    }
}
