using GPSS.Entities.General;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GPSS.ReportParts
{
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

        public int Number { get; private set; }

        public int Priority { get; private set; }

        public double MarkTime { get; private set; }

        public int Assembly { get; private set; }

        public int CurrentBlockIndex { get; private set; }

        public int NextBlockIndex { get; private set; }

        public ReadOnlyDictionary<string, dynamic> Parameters { get; private set; }
    }
}
