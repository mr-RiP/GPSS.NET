using GPSS.Entities.Calculations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.ReportParts
{
    public class SavevalueData
    {
        internal SavevalueData(string name, Savevalue savevalue)
        {
            Name = name;
            Value = savevalue.Value;
            RetryChainCount = savevalue.RetryChain.Count;
        }

        public string Name { get; private set; }

        public dynamic Value { get; private set; }

        public int RetryChainCount { get; private set; }
    }
}
