using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Exceptions
{
    public class GpssException : Exception
    {
        public GpssException(string message) : base(message)
        {
        }

        public GpssException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
