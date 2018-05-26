using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General
{
    internal abstract class Block
    {
        abstract public string GpssName { get; }

        abstract public void Run(Transaction transaction);
    }
}
