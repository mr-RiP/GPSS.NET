using GPSS.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General.RetryChain
{
    internal class TransferTestData : ICloneable
    {
        public TransferMode Mode { get; set; }
        public string PrimaryDestination { get; set; }
        public string SecondaryDestination { get; set; }
        public int Increment { get; set; }

        object ICloneable.Clone() => Clone();
        public TransferTestData Clone() => new TransferTestData
        {
            Mode = Mode,
            PrimaryDestination = PrimaryDestination,
            SecondaryDestination = SecondaryDestination,
            Increment = Increment,
        };
    }
}
