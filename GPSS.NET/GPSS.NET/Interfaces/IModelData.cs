using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Interfaces
{
    internal interface IModelData
    {
        IList<IBlock> Blocks { get; }

        IList<IResource> Resources { get; }

        IList<IFunction> Functions { get; }

        IList<ISavedValue> SavedValues { get; }

        IList<IQueue> Queues { get; }

        IList<ITable> Tables { get; }
    }
}
