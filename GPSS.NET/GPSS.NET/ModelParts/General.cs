using GPSS.Entities.General;
using GPSS.Extensions;
using System;
using System.Collections.Generic;

namespace GPSS.ModelParts
{
    // TODO - Обозначить генераторы, label'ы и задержки
    internal class General : ICloneable
    {
        public List<Block> Blocks { get; private set; } = new List<Block>();

        public Dictionary<string, int> Labels { get; private set; } = new Dictionary<string, int>();

        public List<int> Generators { get; private set; } = new List<int>();

        public object Clone()
        {
            return new General
            {
                Blocks = Blocks.Clone(),
                Labels = new Dictionary<string, int>(Labels, Labels.Comparer),
                Generators = new List<int>(Generators),
            };
        }
    }
}
