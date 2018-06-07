using GPSS.Entities.General;
using GPSS.Entities.General.Transactions;
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

        public Storage(int capacity)
        {
            Capacity = capacity;
        }

        public int Capacity { get; private set; }

        private Dictionary<int, double> usageHistory = new Dictionary<int, double>();
        private double lastUsage = -1.0;

        public LinkedList<StorageDelayTransaction> DelayChain { get; private set; } = new LinkedList<StorageDelayTransaction>();
        public LinkedList<StorageDelayTransaction> RetryChain { get; private set; } = new LinkedList<StorageDelayTransaction>();
        public int OccupiedCapacity { get; private set; } = 0;
        public int UseCount { get; private set; } = 0;
        public bool Available { get; set; } = true;

        public int AvailableCapacity { get => Capacity - OccupiedCapacity; }
        public bool Empty { get => OccupiedCapacity == 0; }
        public bool Full { get => OccupiedCapacity == Capacity; }

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
            {
                double sum = 0.0;
                foreach (var kvp in usageHistory)
                    sum += kvp.Value / kvp.Key;
                return sum / usageHistory.Count;
            }
            else
                return 0.0;
        }

        public void MoveDelayChain(TransactionScheduler chains)
        {
            while (DelayChain.Any())
            {   
                var node = DelayChain.First;
                while (node != null && node.Value.StorageCapacity > AvailableCapacity)
                    node = node.Next;

                if (node != null)
                {
                    DelayChain.Remove(node);
                    OccupiedCapacity += node.Value.StorageCapacity;
                    UseCount++;

                    var transaction = node.Value.InnerTransaction;
                    transaction.Chain = TransactionState.Suspended;
                    transaction.NextBlock++;
                    chains.PlaceInCurrentEvents(transaction);
                }
                else
                    break;
            }
        }

        public void Enter(TransactionScheduler chains, Transaction transaction, int capacity)
        {
            if (!Available)
            {
                chains.CurrentEvents.Remove(transaction);
                transaction.Chain = TransactionState.Suspended;
                chains.PlaceInCurrentEvents(transaction);
            }
            else if (capacity <= AvailableCapacity)
            {
                OccupiedCapacity += capacity;
                transaction.NextBlock++;
            }
            else
            {
                chains.CurrentEvents.Remove(transaction);
                transaction.Chain = TransactionState.Passive;
                PlaceInDelayChain(transaction, capacity);
            }
        }

        private void PlaceInDelayChain(Transaction transaction, int capacity)
        {
            if (transaction.Priority <= DelayChain.Last.Value.Priority)
                DelayChain.AddLast(new StorageDelayTransaction(transaction, capacity));
            else
            {
                var node = DelayChain.First;
                while (node.Value.Priority >= transaction.Priority)
                    node = node.Next;
                DelayChain.AddBefore(node, new StorageDelayTransaction(transaction, capacity));
            }
        }

        public void Leave(int capacity)
        {
            OccupiedCapacity -= capacity;
        }

        public void UpdateUsageHistory(TransactionScheduler system)
        {
            if (lastUsage < 0.0)
                lastUsage = system.RelativeClock;

            if (OccupiedCapacity > 0)
            {
                if (usageHistory.ContainsKey(OccupiedCapacity))
                    usageHistory[OccupiedCapacity] += system.RelativeClock - lastUsage;
                else
                    usageHistory.Add(OccupiedCapacity, system.RelativeClock - lastUsage);
            }

            lastUsage = system.RelativeClock;
        }

        public Storage Clone() => new Storage
        {
            Capacity = Capacity,
            OccupiedCapacity = OccupiedCapacity,
            DelayChain = new LinkedList<StorageDelayTransaction>(DelayChain.Select(sdt => (StorageDelayTransaction)sdt.Clone())),
            UseCount = UseCount,
            Available = Available,

            usageHistory = new Dictionary<int, double>(usageHistory),
            lastUsage = lastUsage,
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
