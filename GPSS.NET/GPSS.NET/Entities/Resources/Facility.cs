using GPSS.Entities.General;
using GPSS.Entities.General.Transactions;
using GPSS.Enums;
using GPSS.Extensions;
using GPSS.SimulationParts;
using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPSS.Entities.Resources
{
	// http://www.minutemansoftware.com/reference/r4.htm#4.3
	internal class Facility : ICloneable, IFacilityAttributes, IResetable, IRetryChainContainer
	{
		private double busyTime = 0.0;
		private double lastUsageTime = -1.0;

		public Transaction Owner { get; private set; } = null;
		public LinkedList<Transaction> DelayChain { get; private set; } = new LinkedList<Transaction>();
		public LinkedList<Transaction> PendingChain { get; private set; } = new LinkedList<Transaction>();
		public LinkedList<FutureEventTransaction> InterruptChain { get; private set; } = new LinkedList<FutureEventTransaction>();
		public LinkedList<RetryChainTransaction> RetryChain { get; private set; } = new LinkedList<RetryChainTransaction>();

		public bool Available { get; private set; } = true;
		public int CaptureCount { get; private set; } = 0;

		public bool Interrupted => InterruptChain.Count > 0;
		public bool Busy => Owner != null;
		public bool Idle => Owner == null;

		public double Utilization()
		{
			if (busyTime > 0.0)
				return busyTime / lastUsageTime * 1000.0;
			else
				return 0.0;
		}

		public double AverageHoldingTime()
		{
			if (CaptureCount > 0)
				return busyTime / CaptureCount;
			else
				return 0.0;
		}

		public Facility Clone() => new Facility
		{
			Owner = Owner,
			DelayChain = DelayChain.Clone(),
			PendingChain = PendingChain.Clone(),
			InterruptChain = InterruptChain.Clone(),
			Available = Available,
			CaptureCount = CaptureCount,

			busyTime = busyTime,
			lastUsageTime = lastUsageTime,
		};
		object ICloneable.Clone() => Clone();

		// http://www.minutemansoftware.com/reference/r7.htm#RELEASE
		public void Release(TransactionScheduler scheduler, Transaction transaction)
		{
			if (transaction == Owner)
			{
				UpdateUsageHistory(scheduler);
				Owner = null;
				MoveChains(scheduler);
				TestRetryChain(scheduler);
			}
			else if (Interrupted && InterruptChain.Any(fe => fe.InnerTransaction == transaction))
			{
				var preemptedTransaction = InterruptChain.First(fe => fe.InnerTransaction == transaction);
				InterruptChain.Remove(preemptedTransaction);
				transaction.PreemptionCount--;
			}
			else
				throw new ArgumentOutOfRangeException(nameof(transaction));
		}

		private void TestRetryChain(TransactionScheduler scheduler)
		{
			RetryChainContainerExtensions.TestRetryChain(this, scheduler);
		}

		private void UpdateCaptureCount()
		{
			if (Busy)
				CaptureCount++;
		}

		// http://www.minutemansoftware.com/reference/r7.htm#SEIZE
		public void Seize(TransactionScheduler scheduler, Transaction transaction)
		{
			if (PreemptedByThis(transaction))
				Refuse(scheduler, transaction);
			else if (Available && Idle)
			{
				UpdateUsageHistory(scheduler);
				Owner = transaction;
				UpdateCaptureCount();
			}
			else
			{
				scheduler.CurrentEvents.Remove(transaction);
				transaction.State = TransactionState.Passive;
				PlaceInDelayChain(scheduler, transaction);
			}
		}

		// http://www.minutemansoftware.com/reference/r7.htm#PREEMPT
		// http://www.minutemansoftware.com/reference/r9.htm#9.4
		public void Preempt(
			TransactionScheduler scheduler,
			Transaction transaction,
			bool priorityMode,
			string parameterName,
			int? newNextBlock,
			bool removeMode)
		{
			if (PreemptedByThis(transaction))
				Refuse(scheduler, transaction);
			else if (Available && Idle)
			{
				UpdateUsageHistory(scheduler);
				Owner = transaction;
				UpdateCaptureCount();
			}
			else if (Available && (priorityMode && transaction.Priority > Owner.Priority || !Interrupted))
			{
				UpdateUsageHistory(scheduler);
				RemoveFromOwnership(scheduler, parameterName, removeMode, newNextBlock);
				Owner = transaction;
				UpdateCaptureCount();
			}
			else if (priorityMode)
				PlaceInDelayChain(scheduler, transaction);
			else
				PlaceInPendingChain(scheduler, transaction);
		}

		private bool PreemptedByThis(Transaction transaction)
		{
			return transaction.Preempted && Interrupted && InterruptChain.Any(t => t.Number == transaction.Number);
		}

		private void Refuse(TransactionScheduler scheduler, Transaction transaction)
		{
			transaction.State = TransactionState.Suspended;
			transaction.NextBlock = transaction.CurrentBlock;

			scheduler.CurrentEvents.Remove(transaction);
			scheduler.PlaceInCurrentEvents(transaction);
		}

		private void RemoveFromOwnership(TransactionScheduler scheduler, string parameterName, bool removeMode, int? newNextBlock)
		{
			if (removeMode && newNextBlock == null)
				throw new ArgumentNullException(nameof(newNextBlock));

			if (newNextBlock.HasValue)
			{
				Owner.NextBlock = newNextBlock.Value;
				RemoveOwnerFromRetryChains(scheduler);
			}

			FutureEventTransaction interrupted = null;
			if (Owner.State == TransactionState.Suspended)
				interrupted = GetInterruptedFromFutureEvents(scheduler, parameterName);
			else if (Owner.State == TransactionState.Passive && newNextBlock.HasValue)
				MoveInterruptedToCurrentEvents(scheduler);

			PlaceInInterruptChain(interrupted, removeMode);
		}

		private void RemoveOwnerFromRetryChains(TransactionScheduler scheduler)
		{
			scheduler.RemoveFromRetryChains(Owner);
		}

		private void MoveInterruptedToCurrentEvents(TransactionScheduler scheduler)
		{
			bool removed = RemoveOwnerFromChains(scheduler.FacilityDelayChains.Values);
			if (!removed)
				removed = RemoveOwnerFromChains(scheduler.FacilityPendingChains.Values);
			if (!removed)
				removed = RemoveOwnerFromChains(scheduler.UserChains.Values);
			if (!removed)
			{
				StorageDelayTransaction transaction = null;
				var chain = scheduler.StorageDelayChains.Values
					.First(c => (transaction = c.FirstOrDefault(t => t.InnerTransaction == Owner)) != null);
			}

			Owner.State = TransactionState.Suspended;
			scheduler.PlaceInCurrentEvents(Owner);
		}

		private bool RemoveOwnerFromChains(ICollection<LinkedList<Transaction>> chains)
		{
			bool removed = false;
			foreach (var chain in chains)
				if (removed = chain.Remove(Owner))
					break;

			return removed;
		}

		private void PlaceInInterruptChain(FutureEventTransaction interrupted, bool removeMode)
		{
			if (!removeMode)
			{
				Owner.PreemptionCount++;
				InterruptChain.AddLast(interrupted ?? new FutureEventTransaction(Owner, 0.0));
			}
		}

		private FutureEventTransaction GetInterruptedFromFutureEvents(TransactionScheduler scheduler, string parameterName)
		{
			var ownerFutureEvent = scheduler.FutureEvents.FirstOrDefault(fe => fe.InnerTransaction == Owner);
			if (ownerFutureEvent != null)
			{
				scheduler.FutureEvents.Remove(ownerFutureEvent);
				scheduler.PlaceInCurrentEvents(Owner);
				ownerFutureEvent.DepartureTime -= scheduler.RelativeTime;
				if (parameterName != null)
					Owner.SetParameter(parameterName, ownerFutureEvent.DepartureTime);
			}

			return ownerFutureEvent;
		}

		private void PlaceInPendingChain(TransactionScheduler scheduler, Transaction transaction)
		{
			scheduler.CurrentEvents.Remove(transaction);
			transaction.State = TransactionState.Passive;

			PendingChain.AddLast(transaction);
		}

		// http://www.minutemansoftware.com/reference/r7.htm#RETURN
		public void Return(TransactionScheduler scheduler, Transaction transaction)
		{
			Release(scheduler, transaction);
		}

		// http://www.minutemansoftware.com/reference/r7.htm#FAVAIL
		public void SetAvailable(TransactionScheduler scheduler)
		{
			Available = true;
			if (Idle)
			{
				UpdateUsageHistory(scheduler);
				MoveChains(scheduler);
				TestRetryChain(scheduler);
			}
		}

		// http://www.minutemansoftware.com/reference/r7.htm#FUNAVAIL
		public void SetUnavailable()
		{
			throw new NotImplementedException();
		}

		// If the Active Transaction gives up ownership of the Facility,
		// the next owner is taken from the Pending Chain,
		// the Interrupt Chain, and finally the Delay Chain.
		private void MoveChains(TransactionScheduler scheduler)
		{
			if (PendingChain.Count != 0)
				ReturnToCurrentEvents(scheduler, PendingChain);
			else if (InterruptChain.Count != 0)
			{
				var interrupedTransaction = InterruptChain.First.Value;
				InterruptChain.RemoveFirst();

				interrupedTransaction.PreemptionCount--;

				if (interrupedTransaction.DepartureTime == 0.0)
					scheduler.PlaceInCurrentEvents(interrupedTransaction.InnerTransaction);
				else
					scheduler.PlaceInFutureEvents(
						interrupedTransaction.InnerTransaction,
						interrupedTransaction.DepartureTime);
			}
			else if (DelayChain.Count != 0)
				ReturnToCurrentEvents(scheduler, DelayChain);
		}

		private void ReturnToCurrentEvents(TransactionScheduler scheduler, LinkedList<Transaction> chain)
		{
			var transaction = chain.First.Value;
			chain.RemoveFirst();

			transaction.State = TransactionState.Suspended;
			scheduler.PlaceInCurrentEvents(transaction);
		}

		private void PlaceInDelayChain(TransactionScheduler scheduler, Transaction transaction)
		{
			scheduler.CurrentEvents.Remove(transaction);
			transaction.State = TransactionState.Passive;

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
			if (lastUsageTime < scheduler.RelativeTime)
			{
				if (lastUsageTime < 0.0)
					lastUsageTime = scheduler.RelativeTime;

				if (Busy)
					busyTime += scheduler.RelativeTime - lastUsageTime;

				lastUsageTime = scheduler.RelativeTime;
			}
		}

		public void Reset()
		{
			busyTime = 0.0;
			lastUsageTime = 0.0;
			CaptureCount = Busy ? 0 : 1;
		}
	}
}
