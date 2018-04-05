using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Interfaces
{
    internal interface ITransact
    {
        int Id { get; set; }

        double Time { get; set; }

        int CurrentBlockIndex { get; set; }

        int TargetBlockIndex { get; set; }

        double TransactionTime { get; set; }

        int Priority { get; set; }

        double CreationTime { get; set; }

        IList<IParameter> Parameters { get; }
    }
}
