using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Extensions
{
    interface ICalculatable<T>
    {
        T Result { get; }

        void Calculate(IStandardAttributes simulation);
    }
}
