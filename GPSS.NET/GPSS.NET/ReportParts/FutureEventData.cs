using GPSS.Entities.General.Transactions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GPSS.ReportParts
{
    /// <summary>
    /// FEC transaction simulation data class.
    /// </summary>
    public class FutureEventData
    {
        internal FutureEventData(FutureEventTransaction futureEvent)
        {
            Number = futureEvent.Number;
            Priority = futureEvent.Priority;
            DepartureTime = futureEvent.DepartureTime;
            Assembly = futureEvent.Assembly;
            CurrentBlockIndex = futureEvent.CurrentBlock;
            NextBlockIndex = futureEvent.NextBlock;
            Parameters = new ReadOnlyDictionary<string, dynamic>(futureEvent.Parameters);
        }

        internal FutureEventData()
        {

        }

        /// <summary>
        /// Transcation Number value.
        /// </summary>
        public int Number { get; internal set; }

        /// <summary>
        /// Transaction Priority value.
        /// </summary>
        public int Priority { get; internal set; }

        /// <summary>
        /// Transaction Departure Time value.
        /// </summary>
        public double DepartureTime { get; internal set; }

        /// <summary>
        /// Transaction Assembly value.
        /// </summary>
        public int Assembly { get; internal set; }

        /// <summary>
        /// Transaction Current Block value.
        /// </summary>
        public int CurrentBlockIndex { get; internal set; }

        /// <summary>
        /// Transaction Next Block value.
        /// </summary>
        public int NextBlockIndex { get; internal set; }

        /// <summary>
        /// Transaction Parameters collection.
        /// </summary>
        public ReadOnlyDictionary<string, dynamic> Parameters { get; internal set; }
    }
}
