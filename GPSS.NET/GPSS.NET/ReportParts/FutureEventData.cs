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

        /// <summary>
        /// Transcation Number value.
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// Transaction Priority value.
        /// </summary>
        public int Priority { get; private set; }

        /// <summary>
        /// Transaction Departure Time value.
        /// </summary>
        public double DepartureTime { get; private set; }

        /// <summary>
        /// Transaction Assembly value.
        /// </summary>
        public int Assembly { get; private set; }

        /// <summary>
        /// Transaction Current Block value.
        /// </summary>
        public int CurrentBlockIndex { get; private set; }

        /// <summary>
        /// Transaction Next Block value.
        /// </summary>
        public int NextBlockIndex { get; private set; }

        /// <summary>
        /// Transaction Parameters collection.
        /// </summary>
        public ReadOnlyDictionary<string, dynamic> Parameters { get; private set; }
    }
}
