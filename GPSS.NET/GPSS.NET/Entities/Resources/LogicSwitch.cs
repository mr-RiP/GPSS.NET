﻿using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.Resources
{
    internal class Logicswitch : ICloneable, ILogicswitchAttributes
    {
        public bool Set => throw new NotImplementedException();

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
