using GPSS.Entities.General;
using GPSS.Enums;
using GPSS.Extensions;
using GPSS.SimulationParts;
using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPSS.Entities.Resources
{
    // http://www.minutemansoftware.com/reference/r6.htm#STORAGE
    // http://www.minutemansoftware.com/reference/r4.htm#4.8
    internal class Storage : ICloneable, IStorageAttributes, IResetable
    {
        private Storage()
        {

        }

        private Storage(Storage storage)
        {
            Capacity = storage.Capacity;
            usageHistory = storage.usageHistory;
            available = storage.available;

        }

        public Storage(int capacity)
        {
            Capacity = capacity;
        }

        public int Capacity { get; private set; }

        private Dictionary<int, double> usageHistory = new Dictionary<int, double>();
        private double lastUsage = 0.0;
        private bool available = true;

        public LinkedList<Transaction> DelayChain { get; private set; } = new LinkedList<Transaction>();
        public int OccupiedCapacity { get; private set; } = 0;
        public int UseCount { get; private set; } = 0;

        public int AvailableCapacity { get => Capacity - OccupiedCapacity; }
        public bool Empty { get => OccupiedCapacity == 0; }
        public bool Full { get => OccupiedCapacity == Capacity; }
        public bool Available
        {
            get => available && OccupiedCapacity < Capacity;
            set => available = value;
        }

        public double AverageCapacity()
        {
            if (usageHistory.Any())
            {
                double sum = 0.0;
                double weightsSum = 0.0;
                foreach (var kvp in usageHistory)
                {
                    sum += kvp.Key * kvp.Value;
                    weightsSum += kvp.Value;
                }

                return sum / weightsSum;
            }
            else
                return 0.0;
        }

        public double Utilization()
        {
            return AverageCapacity() / Capacity * 1000.0;
        }

        public int MaximumCapacityUsed()
        {
            return usageHistory.Keys.Max();
        }

        public double AverageHoldingTime()
        {
            if (usageHistory.Any())
                return usageHistory.Values.Sum() / usageHistory.Count;
            else
                return 0.0;
        }

        public void MoveChain(TransactionChains chains)
        {
            while (Available && DelayChain.Any())
            {
                var transaction = DelayChain.First.Value;
                DelayChain.RemoveFirst();
                transaction.Chain = TransactionState.Suspended;
                chains.PlaceInCurrentEvents(transaction);

                OccupiedCapacity++;
                UseCount++;
            }
        }

        public void UpdateUsageHistory(SystemCounters system)
        {
            if (OccupiedCapacity > 0)
            {
                if (usageHistory.ContainsKey(OccupiedCapacity))
                    usageHistory[OccupiedCapacity] += system.RelativeClock - lastUsage;
                else
                    usageHistory.Add(OccupiedCapacity, system.RelativeClock - lastUsage);
            }
        }

        public Storage Clone() => new Storage
        {
            Capacity = Capacity,
            OccupiedCapacity = OccupiedCapacity,
            DelayChain = new LinkedList<Transaction>(DelayChain),
            UseCount = UseCount,

            usageHistory = new Dictionary<int, double>(usageHistory),
            lastUsage = lastUsage,
            available = available,
        };

        object ICloneable.Clone() => Clone();

        public void Reset()
        {
            UseCount = 0;

            usageHistory.Clear();
            lastUsage = 0.0;
        }
    }
}
