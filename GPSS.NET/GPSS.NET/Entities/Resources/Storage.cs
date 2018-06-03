using GPSS.Extensions;
using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.Resources
{
    internal class Storage : ICloneable, IStorageAttributes
    {
        public int AvailableCapacity => throw new NotImplementedException();

        public int OccupiedCapacity => throw new NotImplementedException();

        public double AverageCapacity => throw new NotImplementedException();

        public int UseCount => throw new NotImplementedException();

        public bool Empty => throw new NotImplementedException();

        public bool Full => throw new NotImplementedException();

        public double Utilization => throw new NotImplementedException();

        public int MaximumCapacityUsed => throw new NotImplementedException();

        public double AverageHoldingTime => throw new NotImplementedException();

        public bool Available => throw new NotImplementedException();

        public Storage Clone()
        {
            throw new NotImplementedException();
        }

        object ICloneable.Clone() => Clone();
    }
}
