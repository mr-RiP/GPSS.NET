using GPSS.Entities.Groups;
using GPSS.Extensions;
using System;
using System.Collections.Generic;

namespace GPSS.ModelParts
{
    internal class Groups : ICloneable
    {
        public Dictionary<string, NumericGroup> NumericGroups { get; private set; } = new Dictionary<string, NumericGroup>();

        public Dictionary<string, TransactionAssembly> TransactionAssemblies { get; private set; } = new Dictionary<string, TransactionAssembly>();

        public Dictionary<string, TransactionGroup> TransactionGroups { get; private set; } = new Dictionary<string, TransactionGroup>();

        public object Clone()
        {
            return new Groups
            {
                NumericGroups = NumericGroups.Clone(),
                TransactionAssemblies = TransactionAssemblies.Clone(),
                TransactionGroups = TransactionGroups.Clone(),
            };
        }
    }
}
