using GPSS.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General.Blocks
{
    internal class Gate : Block
    {
        public override string TypeName => "GATE";

        public override Block Clone()
        {
            throw new NotImplementedException();
        }
    }
}
