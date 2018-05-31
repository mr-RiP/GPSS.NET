using GPSS.Entities.Statistics;
using GPSS.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPSS.ModelParts
{
    internal class Statistics : ICloneable
    {
        public Dictionary<string, Queue> Queues { get; private set; } = new Dictionary<string, Queue>();

        public Dictionary<string, Table> Tables { get; private set; } = new Dictionary<string, Table>();

        public Statistics Clone()=> new Statistics
        {
            Queues = Queues.Clone(),
            Tables = Tables.Clone()
        };

        public void Clear()
        {
            throw new NotImplementedException();
        }

        object ICloneable.Clone() => Clone();
    }
}
