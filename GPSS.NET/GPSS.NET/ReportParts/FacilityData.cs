using GPSS.Entities.Resources;
using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.ReportParts
{
    public class FacilityData
    {
        internal FacilityData(string name, Facility facility)
        {
            Name = name;
            CaptureCount = facility.CaptureCount;
            Available = facility.Available;
            AverageHoldingTime = facility.AverageHoldingTime();
            Utilization = facility.Utilization();
            OwnerNumber = facility.Owner?.Number;
            InterruptChainCount = facility.InterruptChain.Count;
            PendingChainCount = facility.PendingChain.Count;
            DelayChainCount = facility.DelayChain.Count;
            RetryChainCount = facility.RetryChain.Count;
        }

        public string Name { get; private set; }

        public int CaptureCount { get; private set; }

        public bool Available { get; private set; }

        public double AverageHoldingTime { get; private set; }

        public double Utilization { get; private set; }

        public int? OwnerNumber { get; private set; }

        public int InterruptChainCount { get; private set; }

        public int PendingChainCount { get; private set; }

        public int DelayChainCount { get; private set; }

        public int RetryChainCount { get; private set; }
    }
}
