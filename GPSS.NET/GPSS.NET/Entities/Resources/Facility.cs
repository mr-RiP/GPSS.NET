using GPSS.Entities.General;
using GPSS.Enums;
using GPSS.Extensions;
using GPSS.SimulationParts;
using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.Resources
{
    // http://www.minutemansoftware.com/reference/r4.htm#4.3
    internal class Facility : ICloneable, IFacilityAttributes, IResetable
    {
        private double busyTime = 0.0;
        private double lastUsageTime = -1.0;

        public Transaction Owner { get; private set; } = null;
        public LinkedList<Transaction> DelayChain { get; private set; } = new LinkedList<Transaction>();
        public LinkedList<Transaction> InterruptChain { get; private set; } = new LinkedList<Transaction>();
        public LinkedList<Transaction> PendingChain { get; private set; } = new LinkedList<Transaction>();

        public bool Available { get; private set; } = true;
        public int CaptureCount { get; private set; } = 0;

        public bool Interrupted => InterruptChain.Count > 0;
        public bool Busy => Owner != null;
        public bool Idle => Owner == null;
        public double Utilization() => throw new NotImplementedException();

        public double AverageHoldingTime()
        {
            if (CaptureCount > 0)
                return busyTime / CaptureCount;
            else
                return 0.0;
        }

        public Facility Clone()
        {
            throw new NotImplementedException();
        }
        object ICloneable.Clone() => Clone();

        // http://www.minutemansoftware.com/reference/r7.htm#RELEASE
        public void Release(TransactionScheduler scheduler, Transaction transaction)
        {
            if (transaction == Owner)
                NextOwner(scheduler);
            else if (Interrupted && InterruptChain.Contains(transaction))
                InterruptChain.Remove(transaction);
            else
                throw new ArgumentOutOfRangeException(nameof(transaction));
        }

        // http://www.minutemansoftware.com/reference/r7.htm#SEIZE
        public void Seize(TransactionScheduler scheduler, Transaction transaction)
        {
            if (Available && Idle)
                Owner = transaction;
            else
            {
                scheduler.CurrentEvents.Remove(transaction);
                transaction.State = TransactionState.Passive;
                PlaceInDelayChain(transaction);
            }
        }

        // http://www.minutemansoftware.com/reference/r7.htm#PREEMPT
        // http://www.minutemansoftware.com/reference/r9.htm#9.4
        public void Preempt(TransactionScheduler scheduler, Transaction transaction, bool priorityMode, int? newNextBlock, string parameterName, bool removeMode)
        {
            if (Available && Idle)
                Owner = transaction;
            else if (priorityMode)
                PreemptPriorityMode(scheduler, transaction, newNextBlock, parameterName, removeMode);
            else
                PreemptInterruptMode(scheduler, transaction, newNextBlock, parameterName, removeMode);
        }

        private void PreemptInterruptMode(TransactionScheduler scheduler, Transaction transaction, int? newNextBlock, string parameterName, bool removeMode)
        {
            throw new NotImplementedException();
        }

        private void PreemptPriorityMode(TransactionScheduler scheduler, Transaction transaction, int? newNextBlock, string parameterName, bool removeMode)
        {
            throw new NotImplementedException();
        }

        // http://www.minutemansoftware.com/reference/r7.htm#RETURN
        public void Return(TransactionScheduler scheduler, Transaction transaction)
        {
            Release(scheduler, transaction);
        }

        // http://www.minutemansoftware.com/reference/r7.htm#FAVAIL
        public void SetAvailable()
        {
            throw new NotImplementedException();
        }

        // http://www.minutemansoftware.com/reference/r7.htm#FUNAVAIL
        public void SetUnavailable()
        {
            throw new NotImplementedException();
        }

        // If the Active Transaction gives up ownership of the Facility,
        // the next owner is taken from the Pending Chain,
        // the Interrupt Chain, and finally the Delay Chain.
        private void NextOwner(TransactionScheduler scheduler)
        {
            if (PendingChain.Count != 0)
            {
                Owner = PendingChain.First.Value;
                PendingChain.RemoveFirst();
            }
            else if (InterruptChain.Count != 0)
            {
                Owner = InterruptChain.First.Value;
                InterruptChain.RemoveFirst();
            }
            else if (DelayChain.Count != 0)
            {
                Owner = DelayChain.First.Value;
                DelayChain.RemoveFirst();
            }
            else
                Owner = null;
        }

        private void PlaceInDelayChain(Transaction transaction)
        {
            if (transaction.Priority <= DelayChain.Last.Value.Priority)
                DelayChain.AddLast(transaction);
            else
            {
                var node = DelayChain.First;
                while (node.Value.Priority >= transaction.Priority)
                    node = node.Next;
                DelayChain.AddBefore(node, transaction);
            }
        }

        public void UpdateUsageHistory(TransactionScheduler scheduler)
        {
            if (lastUsageTime < 0.0)
                lastUsageTime = scheduler.RelativeClock;

            if (Busy)
                busyTime += scheduler.RelativeClock - lastUsageTime;
        }

        public void Reset()
        {
            busyTime = 0.0;
            lastUsageTime = 0.0;
            CaptureCount = Busy ? 0 : 1;
        }
    }
}
