using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General
{
    internal abstract class Command : ICloneable
    {
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
