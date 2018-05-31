using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.Resources
{
    internal class Storage : ICloneable, ILogicSwitchAttributes
    {
        public bool Set => throw new NotImplementedException();

        public Storage Clone()
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
