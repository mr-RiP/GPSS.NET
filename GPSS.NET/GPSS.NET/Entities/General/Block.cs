using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General
{
    internal abstract class Block : ICloneable
    {
        abstract public string TypeName { get; }

        abstract public object Clone();

        abstract public void Run(Simulation s);
    }
}
