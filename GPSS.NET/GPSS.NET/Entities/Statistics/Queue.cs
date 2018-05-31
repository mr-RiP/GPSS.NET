using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.Statistics
{
    internal class Queue : ICloneable, IQueueAttributes
    {
        public int Content => throw new NotImplementedException();

        public double AverageContent => throw new NotImplementedException();

        public int MaxContent => throw new NotImplementedException();

        public int EntryCount => throw new NotImplementedException();

        public int ZeroEntryCount => throw new NotImplementedException();

        public double AverageResidenceTime => throw new NotImplementedException();

        public double AverageNonZeroResidenceTime => throw new NotImplementedException();

        public Queue Clone()
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
