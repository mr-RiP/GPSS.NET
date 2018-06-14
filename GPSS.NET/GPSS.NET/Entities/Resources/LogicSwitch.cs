using GPSS.Entities.General.Transactions;
using GPSS.Extensions;
using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.Resources
{
    internal class Logicswitch : ICloneable, ILogicswitchAttributes, IRetryChainContainer
    {
        public bool Set => throw new NotImplementedException();

        public LinkedList<RetryChainTransaction> RetryChain => throw new NotImplementedException();

        public Logicswitch Clone()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        object ICloneable.Clone() => Clone();
    }
}
