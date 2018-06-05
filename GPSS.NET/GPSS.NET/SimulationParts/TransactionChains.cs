using GPSS.Entities.General;
using GPSS.Entities.General.Transactions;
using GPSS.Entities.Groups;
using GPSS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPSS.SimulationParts
{
    // http://www.minutemansoftware.com/reference/r9.htm#9.1
    internal class TransactionChains
    {
        public Dictionary<string, LinkedList<Transaction>> UserChains { get; private set; } = new Dictionary<string, LinkedList<Transaction>>();
        
        public Dictionary<string, LinkedList<Transaction>> StorageDelayChains { get; private set; } = new Dictionary<string, LinkedList<Transaction>>();

        public Dictionary<string, LinkedList<Transaction>> FacilityDelayChains { get; private set; } = new Dictionary<string, LinkedList<Transaction>>();

        public Dictionary<string, LinkedList<Transaction>> FacilityPendingChains { get; private set; } = new Dictionary<string, LinkedList<Transaction>>();

        public LinkedList<FutureEventTransaction> FutureEvents { get; private set; } = new LinkedList<FutureEventTransaction>();

        // Kept in priority order, the Active Transaction is usually returned to the CEC ahead of its peers.
        // When the Active Transaction comes to rest on some Transaction Chain,
        // the highest priority Transaction remaining on the CEC becomes the Active Transaction.
        public LinkedList<Transaction> CurrentEvents { get; private set; } = new LinkedList<Transaction>();

        public double CurrentTimeIncrement { get; private set; } = 0.0;

        public Transaction GetActiveTransaction()
        {
            return CurrentEvents.First.Value;
        }

        public void PlaceInCurrentEvents(Transaction transaction)
        {
            if (CurrentEvents.Count == 0 || transaction.Priority <= CurrentEvents.Last.Value.Priority)
                CurrentEvents.AddLast(transaction);
            else
            {
                var node = CurrentEvents.First;
                while (node.Value.Priority >= transaction.Priority)
                    node = node.Next;
                CurrentEvents.AddBefore(node, transaction);
            }
        }

        public void PlaceInFutureEvents(Transaction transaction, double timeIncrement)
        {
            var futureEvent = new FutureEventTransaction(transaction, timeIncrement + CurrentTimeIncrement);
            if (FutureEvents.Count == 0 || futureEvent.TimeIncrement >= FutureEvents.Last.Value.TimeIncrement)
                FutureEvents.AddLast(futureEvent);
            else
            {
                var node = FutureEvents.First;
                while (node.Value.TimeIncrement <= futureEvent.TimeIncrement)
                    node = node.Next;
                FutureEvents.AddBefore(node, futureEvent);
            }
        }

        public void UpdateEvents()
        {
            CurrentTimeIncrement = FutureEvents.First.Value.TimeIncrement;
            while (FutureEvents.First.Value.TimeIncrement == CurrentTimeIncrement)
            {
                PlaceInCurrentEvents(FutureEvents.First.Value.InnerTransaction);
                FutureEvents.RemoveFirst();
            }       
        }

        public void RefreshTimeIncrement()
        {
            if (CurrentEvents.Count != 0)
            {
                foreach (var transaction in FutureEvents)
                    transaction.TimeIncrement -= CurrentTimeIncrement;
                CurrentTimeIncrement = 0.0;
            }
        }
    }
}
