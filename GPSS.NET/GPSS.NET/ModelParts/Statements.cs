using GPSS.Entities.General;
using GPSS.Entities.General.Blocks;
using GPSS.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPSS.ModelParts
{
    internal class Statements : ICloneable
    {
        public List<Block> Blocks { get; private set; } = new List<Block>();

        public Dictionary<Generate, int> Generators { get; private set; } = new Dictionary<Generate, int>();

        public Dictionary<Transfer, int> Transfers { get; private set; } = new Dictionary<Transfer, int>();

        public Dictionary<Gate, int> Gates { get; private set; } = new Dictionary<Gate, int>();

        public Dictionary<string, int> Labels { get; private set; } = new Dictionary<string, int>();

        public Statements Clone()
        {
            List<Block> cloneBlocks = Blocks.Clone();
            return new Statements
            {
                Blocks = cloneBlocks,
                Labels = new Dictionary<string, int>(Labels, Labels.Comparer),
                Generators = Generators.CloneMap(cloneBlocks),
                Gates = Gates.CloneMap(cloneBlocks),
                Transfers = Transfers.CloneMap(cloneBlocks),
            };
        }

        object ICloneable.Clone() => Clone();

        public bool ContainsInRetryChains(Transaction transaction)
        {
            return RetryChainsExpression(Transfers.Keys, (r => r.Contains(transaction))) ||
                RetryChainsExpression(Gates.Keys, (r => r.Contains(transaction)));
        }

        public bool RemoveFromRetryChains(Transaction transaction)
        {
            return RetryChainsExpression(Transfers.Keys, (r => r.Remove(transaction))) ||
                RetryChainsExpression(Gates.Keys, (r => r.Remove(transaction)));
        }

        private bool RetryChainsExpression<T>(
            ICollection<T> collection,
            Func<IRetryChainContainer, bool> expression) where T : IRetryChainContainer
        {
            bool found = false;
            foreach (var chain in collection)
                if (found = expression(chain))
                    break;
            return found;
        }

    }
}
