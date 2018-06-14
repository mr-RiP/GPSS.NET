using GPSS.Entities.General.Transactions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GPSS.ReportParts
{
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

        public int Number { get; private set; }

        public int Priority { get; private set; }

        public double DepartureTime { get; private set; }

        public int Assembly { get; private set; }

        public int CurrentBlockIndex { get; private set; }

        public int NextBlockIndex { get; private set; }

        public ReadOnlyDictionary<string, dynamic> Parameters { get; private set; }
    }
}
