using GPSS.Entities.General;
using GPSS.ModelParts;
using GPSS.StandardAttributes;
using System.Collections.ObjectModel;
using System.Linq;

namespace GPSS.ReportParts
{
    /// <summary>
    /// Model Block's simulation data class.
    /// </summary>
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

        /// <summary>
        /// Block Index.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Block label names.
        /// </summary>
        public ReadOnlyCollection<string> Names { get; private set; }

        /// <summary>
        /// The GPSS Block Type Name.
        /// </summary>
        public string TypeName { get; private set; }

        /// <summary>
        /// Block's transactions entry count.
        /// </summary>
        public int EntryCount { get; private set; }

        /// <summary>
        /// Block's current transactions count.
        /// </summary>
        public int TransactionsCount { get; private set; }

        /// <summary>
        /// Block's Retry Chain content count.
        /// </summary>
        public int RetryCount { get; private set; }
    }
}
