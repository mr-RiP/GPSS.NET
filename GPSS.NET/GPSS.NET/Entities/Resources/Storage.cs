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
    // Касательно RETRY Chain в референсе написана хуйня
    // На практике при невозможности обработки - все идет в Delay Chain
    // https://i.imgur.com/xzR03MR.png
    internal class Storage : ICloneable, IStorageAttributes, IResetable
    {
        private Storage()
        {

        }

        public Storage(int capacity)
        {
            Capacity = capacity;
        }

        private Dictionary<int, double> usageHistory = new Dictionary<int, double>();
        private double lastUsage = -1.0;

        public int Capacity { get; private set; }
        public LinkedList<StorageDelayTransaction> DelayChain { get; private set; } = new LinkedList<StorageDelayTransaction>();
        public int OccupiedCapacity { get; private set; } = 0;
        public int UseCount { get; private set; } = 0;
        public bool Available { get; private set; } = true;

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

        public void SetAvailable(TransactionScheduler scheduler)
        {
            Available = true;
            if (!Full)
            {
                UpdateUsageHistory(scheduler);
                MoveDelayChain(scheduler);
            }
        }

        public void SetUnavailable()
        {
            Available = false;
        }

        private void MoveDelayChain(TransactionScheduler scheduler)
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
                    transaction.State = TransactionState.Suspended;
                    scheduler.PlaceInCurrentEvents(transaction);
                }
                else
                    break;
            }
        }

        public void Enter(TransactionScheduler scheduler, Transaction transaction, int capacity)
        {
            if (capacity > Capacity)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            if (Available && capacity <= AvailableCapacity)
            {
                UpdateUsageHistory(scheduler);
                OccupiedCapacity += capacity;
            }
            else
            {
                scheduler.CurrentEvents.Remove(transaction);
                transaction.State = TransactionState.Passive;
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

        public void Leave(TransactionScheduler scheduler, int capacity)
        {
            if (capacity > OccupiedCapacity)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            UpdateUsageHistory(scheduler);
            OccupiedCapacity -= capacity;
            if (Available)
                MoveDelayChain(scheduler);
        }

        public void UpdateUsageHistory(TransactionScheduler scheduler)
        {
            if (lastUsage < scheduler.RelativeClock)
            {
                if (lastUsage < 0.0)
                    lastUsage = scheduler.RelativeClock;

                if (OccupiedCapacity > 0)
                {
                    if (usageHistory.ContainsKey(OccupiedCapacity))
                        usageHistory[OccupiedCapacity] += scheduler.RelativeClock - lastUsage;
                    else
                        usageHistory.Add(OccupiedCapacity, scheduler.RelativeClock - lastUsage);
                }

                lastUsage = scheduler.RelativeClock;
            }
        }

        public Storage Clone() => new Storage
        {
            Capacity = Capacity,
            OccupiedCapacity = OccupiedCapacity,
            DelayChain = DelayChain.Clone(),
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
