using GPSS.Entities.General;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GPSS.ReportParts
{
    /// <summary>
    /// CEC transaction simulation data.
    /// </summary>
    public class CurrentEventData
    {
        internal CurrentEventData(Transaction transaction)
        {
            Number = transaction.Number;
            Priority = transaction.Priority;
            MarkTime = transaction.MarkTime;
            Assembly = transaction.Assembly;
            CurrentBlockIndex = transaction.CurrentBlock;
            NextBlockIndex = transaction.NextBlock;
            Parameters = new ReadOnlyDictionary<string, dynamic>(transaction.Parameters);
        }

        internal CurrentEventData()
        { }

        /// <summary>
        /// Transcation Number value.
        /// </summary>
        public int Number { get; internal set; }

        /// <summary>
        /// Transaction Priority value.
        /// </summary>
        public int Priority { get; internal set; }

        /// <summary>
        /// Transaction Mark Time value.
        /// </summary>
        public double MarkTime { get; internal set; }

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
