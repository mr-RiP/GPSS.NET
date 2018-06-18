using GPSS.Entities.Calculations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.ReportParts
{
    /// <summary>
    /// Savevalue simulation data class.
    /// </summary>
    public class SavevalueData
    {
        internal SavevalueData(string name, Savevalue savevalue)
        {
            Name = name;
            Value = savevalue.Value;
            RetryChainCount = savevalue.RetryChain.Count;
        }

        internal SavevalueData()
        {

        }

        /// <summary>
        /// Savevalue Entity name.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Savevalue value.
        /// </summary>
        public dynamic Value { get; internal set; }

        /// <summary>
        /// Savevalue's Retry Chain content count.
        /// </summary>
        public int RetryChainCount { get; internal set; }
    }
}
