using GPSS.Entities.General;
using GPSS.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPSS.ModelParts
{
    internal class General : ICloneable
    {
        public List<Block> Blocks { get; private set; } = new List<Block>();

        public List<Block> Generators { get; private set; } = new List<Block>();

        public Dictionary<string, int> Labels { get; private set; } = new Dictionary<string, int>();

        public General Clone()
        {
            List<Block> cloneBlocks = Blocks.Clone();
            return new General
            {
                Blocks = cloneBlocks,
                Labels = new Dictionary<string, int>(Labels, Labels.Comparer),
                Generators = Generators
                    .Select(g => Blocks.IndexOf(g))
                    .Select(i => cloneBlocks[i])
                    .ToList(),
            };
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        object ICloneable.Clone() => Clone();
    }
}
