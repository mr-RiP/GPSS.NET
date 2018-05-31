using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.Resources
{
    internal class Facility : ICloneable, IFacilityAttributes
    {
        public bool Busy => throw new NotImplementedException();

        public int CaptureCount => throw new NotImplementedException();

        public bool Interrupted => throw new NotImplementedException();

        public double Utilization => throw new NotImplementedException();

        public double AverageHoldingTime => throw new NotImplementedException();

        public bool Available => throw new NotImplementedException();

        public Facility Clone()
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
