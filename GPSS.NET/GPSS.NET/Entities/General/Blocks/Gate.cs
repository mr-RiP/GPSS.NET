using GPSS.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General.Blocks
{
    internal class Gate : Block, IRetryChainContainer
    {
        public override string TypeName => throw new NotImplementedException();

        public override Block Clone()
        {
            throw new NotImplementedException();
        }

        public bool Contains(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
