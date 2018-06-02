using GPSS.Entities.General;
using GPSS.Entities.Groups;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPSS.SimulationParts
{
    // http://www.minutemansoftware.com/reference/r9.htm#9.1
    internal class TransactionChains
    {   
        public Dictionary<string, Userchain> UserChains { get; private set; } = new Dictionary<string, Userchain>();

        public Dictionary<int, List<Transaction>> DelayChains { get; private set; } = new Dictionary<int, List<Transaction>>();

        public Dictionary<int, List<Transaction>> PendingChains { get; private set; } = new Dictionary<int, List<Transaction>>();

        public List<Transaction> FutureEvents { get; private set; } = new List<Transaction>();

        // Kept in priority order, the Active Transaction is usually returned to the CEC ahead of its peers.
        // When the Active Transaction comes to rest on some Transaction Chain,
        // the highest priority Transaction remaining on the CEC becomes the Active Transaction.
        public List<Transaction> CurrentEvents { get; private set; } = new List<Transaction>();

        public Transaction GetActiveTransaction()
        {
            return CurrentEvents.LastOrDefault();
        }

        public void PlaceInCurrentEvents(Transaction transaction)
        {
            int index = CurrentEvents.FindIndex(t => t.Priority >= transaction.Priority);
            CurrentEvents.Insert(index, transaction);
        }

        public void PlaceInFutureEvents(Transaction transaction)
        {
            FutureEvents.Add(transaction);
        }

        public void UpdateCurrentEvents()
        {
            double minTime = FutureEvents.Min(t => t.TimeIncrement);
            CurrentEvents = FutureEvents
                .Where(t => t.TimeIncrement == minTime)
                .OrderBy(t => t.Priority)
                .ToList();
        }

        internal void Clear()
        {
            UserChains.Clear();
            DelayChains.Clear();
            PendingChains.Clear();
            FutureEvents.Clear();
            CurrentEvents.Clear();
        }

        public void RefreshTimeIncrement()
        {
            if (CurrentEvents.Any())
            {
                double time = CurrentEvents.First().TimeIncrement;
                CurrentEvents.ForEach(t => t.TimeIncrement = 0.0);
                FutureEvents.ForEach(t => t.TimeIncrement -= time);
            }
        }

        public double CurrentTimeIncrement()
        {
            return CurrentEvents.FirstOrDefault()?.TimeIncrement ?? 0.0;
        }
    }
}
