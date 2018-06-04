using GPSS.Entities.General;
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

        public LinkedList<Transaction> FutureEvents { get; private set; } = new LinkedList<Transaction>();

        // Kept in priority order, the Active Transaction is usually returned to the CEC ahead of its peers.
        // When the Active Transaction comes to rest on some Transaction Chain,
        // the highest priority Transaction remaining on the CEC becomes the Active Transaction.
        public LinkedList<Transaction> CurrentEvents { get; private set; } = new LinkedList<Transaction>();

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

        public void PlaceInFutureEvents(Transaction transaction)
        {
            if (FutureEvents.Count == 0 || transaction.TimeIncrement >= FutureEvents.Last.Value.TimeIncrement)
                FutureEvents.AddLast(transaction);
            else
            {
                var node = FutureEvents.First;
                while (node.Value.TimeIncrement <= transaction.TimeIncrement)
                    node = node.Next;
                FutureEvents.AddBefore(node, transaction);
            }
        }

        public void UpdateEvents()
        {
            double minTime = FutureEvents.First.Value.TimeIncrement;
            while (FutureEvents.First.Value.TimeIncrement == minTime)
            {
                PlaceInCurrentEvents(FutureEvents.First.Value);
                FutureEvents.RemoveFirst();
            }       
        }

        public void RefreshTimeIncrement()
        {
            if (CurrentEvents.Count != 0)
            {
                double increment = CurrentEvents.First.Value.TimeIncrement;
                foreach (var transaction in CurrentEvents)
                    transaction.TimeIncrement = 0.0;
                foreach (var transaction in FutureEvents)
                    transaction.TimeIncrement -= increment;
            }
        }

        public double CurrentTimeIncrement()
        {
            return CurrentEvents.FirstOrDefault()?.TimeIncrement ?? 0.0;
        } 
    }
}
