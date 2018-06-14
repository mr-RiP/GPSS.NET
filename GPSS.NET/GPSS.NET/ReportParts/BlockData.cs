using GPSS.Entities.General;
using GPSS.ModelParts;
using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace GPSS.ReportParts
{
    public class BlockData : IBlockAttributes
    {
        internal BlockData(Statements statements, int index)
        {
            Index = index;
            CopyBlockStats(statements.Blocks[index]);
            CopyBlockNames(statements, index);
        }

        private void CopyBlockNames(Statements statements, int index)
        {
            Names = statements.Labels
                .Where(kvp => kvp.Value == index)
                .Select(kvp => kvp.Key)
                .ToList()
                .AsReadOnly();
        }

        private void CopyBlockStats(Block block)
        {
            TypeName = block.TypeName;
            EntryCount = block.EntryCount;
            TransactionsCount = block.TransactionsCount;
            RetryCount = block.RetryCount;
        }

        public int Index { get; private set; }

        public ReadOnlyCollection<string> Names { get; private set; }

        public string TypeName { get; private set; }

        public int EntryCount { get; private set; }

        public int TransactionsCount { get; private set; }

        public int RetryCount { get; private set; }
    }
}
