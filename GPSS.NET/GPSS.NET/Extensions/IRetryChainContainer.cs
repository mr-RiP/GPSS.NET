using GPSS.Entities.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Extensions
{
    internal interface IRetryChainContainer
    {
        bool Contains(Transaction transaction);

        bool Remove(Transaction transaction);
    }
}
