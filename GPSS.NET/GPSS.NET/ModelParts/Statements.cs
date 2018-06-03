using GPSS.Entities.General;
using GPSS.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPSS.ModelParts
{
    internal class Statements : ICloneable
    {
        public List<Block> Blocks { get; private set; } = new List<Block>();

        public List<Block> Generators { get; private set; } = new List<Block>();

        public Dictionary<string, int> Labels { get; private set; } = new Dictionary<string, int>();

        public Statements Clone()
        {
            List<Block> cloneBlocks = Blocks.Clone();
            return new Statements
            {
                Blocks = cloneBlocks,
                Labels = new Dictionary<string, int>(Labels, Labels.Comparer),
                Generators = Generators
                    .Select(g => Blocks.IndexOf(g))
                    .Select(i => cloneBlocks[i])
                    .ToList(),
            };
        }

        object ICloneable.Clone() => Clone();
    }
}
