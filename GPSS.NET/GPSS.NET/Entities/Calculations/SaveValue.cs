using GPSS.Entities.General.Transactions;
using GPSS.Extensions;
using GPSS.SimulationParts;
using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;

namespace GPSS.Entities.Calculations
{
    internal class Savevalue : ICloneable, ISavevalueAttributes, IRetryChainContainer
    {
        public dynamic Value { get; set; }

        public LinkedList<RetryChainTransaction> RetryChain { get; private set; } = new LinkedList<RetryChainTransaction>();

        public Savevalue Clone() => new Savevalue
        {
            Value = Value,
        };

        public void TestRetryChain(TransactionScheduler scheduler)
        {

        }

        object ICloneable.Clone() => Clone();
    }
}
