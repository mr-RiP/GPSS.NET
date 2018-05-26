using GPSS.Entities.Groups;
using System.Collections.Generic;

namespace GPSS.Modeling
{
    internal class Groups
    {
        public Dictionary<string, NumericGroup> NumericGroups { get; } = new Dictionary<string, NumericGroup>();

        public Dictionary<string, TransactionAssembly> TransactionAssemblies { get; } = new Dictionary<string, TransactionAssembly>();

        public Dictionary<string, TransactionGroup> TransactionGroups { get; } = new Dictionary<string, TransactionGroup>();
    }
}
