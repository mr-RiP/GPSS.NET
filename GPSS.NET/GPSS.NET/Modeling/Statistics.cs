using GPSS.Entities.Statistics;
using System.Collections.Generic;

namespace GPSS.Modeling
{
    internal class Statistics
    {
        public Dictionary<string, Queue> Queues { get; } = new Dictionary<string, Queue>();

        public Dictionary<string, Table> Tables { get; } = new Dictionary<string, Table>();
    }
}
