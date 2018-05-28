using GPSS.Entities.General;
using System;
using System.Collections.Generic;

namespace GPSS.ModelParts
{
    // TODO - Обозначить генераторы, label'ы и задержки
    // Установить общую модельную связь между блоками и командами?
    internal class General : ICloneable
    {
        public List<Block> Blocks { get; private set; } = new List<Block>();

        public Dictionary<string, Block> LabeledBlocks { get; private set; } = new Dictionary<string, Block>();

        public List<Command> Commands { get; private set; } = new List<Command>();

        public List<Transaction> Transactions { get; private set; } = new List<Transaction>();

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
