using GPSS.Entities.General;
using GPSS.Extensions;
using GPSS.SimulationParts;
using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.Resources
{
    // http://www.minutemansoftware.com/reference/r4.htm#4.3
    internal class Facility : ICloneable, IFacilityAttributes, IResetable
    {
        private double busyTime = 0.0;
        private double lastUsageTime = -1.0;

        public Transaction Owner { get; set; } = null;
        public LinkedList<Transaction> DelayChain { get; set; } = new LinkedList<Transaction>();

        public bool Interrupted { get; set; } = false;
        public int CaptureCount { get; set; } = 0;

        public bool Busy => Owner != null;
        public bool Available => Owner == null;
        public double Utilization() => throw new NotImplementedException();
        public double AverageHoldingTime() => CaptureCount > 0 ? busyTime / CaptureCount : 0.0;

        public Facility Clone()
        {
            throw new NotImplementedException();
        }

        object ICloneable.Clone() => Clone();

        public void MoveChains(TransactionScheduler chains)
        {

        }

        public void Release()
        {

        }

        public void Seize(TransactionScheduler chains, Transaction transaction)
        {

        }

        public void UpdateUsageHistory(TransactionScheduler scheduler)
        {

        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
