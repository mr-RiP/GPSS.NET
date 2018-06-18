﻿using GPSS.Entities.Resources;
using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.ReportParts
{
    /// <summary>
    /// Facility Entity simulation data class.
    /// </summary>
    public class FacilityData
    {
        internal FacilityData(string name, Facility facility)
        {
            Name = name;
            EntryCount = facility.CaptureCount;
            Available = facility.Available;
            AverageHoldingTime = facility.AverageHoldingTime();
            Utilization = facility.Utilization();
            OwnerNumber = facility.Owner?.Number;
            InterruptChainCount = facility.InterruptChain.Count;
            PendingChainCount = facility.PendingChain.Count;
            DelayChainCount = facility.DelayChain.Count;
            RetryChainCount = facility.RetryChain.Count;
        }

        internal FacilityData()
        {

        }

        /// <summary>
        /// Facility Entity name.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Facility capture count.
        /// </summary>
        public int EntryCount { get; internal set; }

        /// <summary>
        /// Facility Available flag.
        /// </summary>
        public bool Available { get; internal set; }

        /// <summary>
        /// Facility Average Holding Time. 
        /// </summary>
        public double AverageHoldingTime { get; internal set; }

        /// <summary>
        /// Facility Utilization value.
        /// </summary>
        public double Utilization { get; internal set; }

        /// <summary>
        /// Facility owner transaction Number. Null if facility is Idle.
        /// </summary>
        public int? OwnerNumber { get; internal set; }

        /// <summary>
        /// Facility's Interrupt Chain content count.
        /// </summary>
        public int InterruptChainCount { get; internal set; }

        /// <summary>
        /// Facility's Pending Chain content count.
        /// </summary>
        public int PendingChainCount { get; internal set; }

        /// <summary>
        /// Facility's Delay Chain content count.
        /// </summary>
        public int DelayChainCount { get; internal set; }

        /// <summary>
        /// Facility's Retry Chain content count.
        /// </summary>
        public int RetryChainCount { get; internal set; }
    }
}
