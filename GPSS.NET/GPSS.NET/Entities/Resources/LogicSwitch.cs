using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.Resources
{
    internal class LogicSwitch : ICloneable, ILogicSwitchAttributes
    {
        public bool Set => throw new NotImplementedException();

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
