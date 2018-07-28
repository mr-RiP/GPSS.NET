using GPSS.Enums;
using GPSS.Exceptions;
using GPSS.Extensions;
using GPSS.SimulationParts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPSS.Entities.General.Blocks
{
	internal class Transfer : Block
	{
		private Transfer()
		{

		}

		public Transfer(
			Func<IStandardAttributes, TransferMode> mode,
			Func<IStandardAttributes, double> fraction,
			Func<IStandardAttributes, string> primaryDestination,
			Func<IStandardAttributes, string> secondaryDestination,
			Func<IStandardAttributes, int> increment)
		{
			Mode = mode;
			Fraction = fraction;
			PrimaryDestination = primaryDestination;
			SecondaryDestination = secondaryDestination;
			Increment = increment;
		}

		public Func<IStandardAttributes, TransferMode> Mode { get; private set; }
		public Func<IStandardAttributes, double> Fraction { get; private set; }
		public Func<IStandardAttributes, string> PrimaryDestination { get; private set; }
		public Func<IStandardAttributes, string> SecondaryDestination { get; private set; }
		public Func<IStandardAttributes, int> Increment { get; private set; }

		public override string TypeName => "TRANSFER";

		public override void EnterBlock(Simulation simulation)
		{
			base.EnterBlock(simulation);
			var transaction = simulation.ActiveTransaction.Transaction;
			try
			{
				Resolve(simulation, transaction, Mode(simulation.StandardAttributes));
			}
			catch (ArgumentNullException error)
			{
				throw new ModelStructureException(
					"Attempt to perform transfer test with null argument.",
					transaction.CurrentBlock,
					error);
			}
			catch (KeyNotFoundException error)
			{
				throw new ModelStructureException(
					"Attempt to perform transfer test with non existent Block name.",
					transaction.CurrentBlock,
					error);
			}
		}

		private void RemoveFromCurrentEvents(Simulation simulation, Transaction transaction)
		{
			simulation.Scheduler.CurrentEvents.Remove(transaction);
		}

		private void Reset(TransactionScheduler scheduler, Transaction transaction)
		{
			transaction.State = TransactionState.Suspended;
			scheduler.CurrentEvents.Remove(transaction);
			scheduler.PlaceInCurrentEvents(transaction);
		}

		private void Resolve(Simulation simulation, Transaction transaction, TransferMode mode)
		{
			switch (mode)
			{
				case TransferMode.Unconditional:
					TransferUnconditional(simulation, transaction);
					break;
				case TransferMode.Fractional:
					TransferFractional(simulation, transaction);
					break;
				case TransferMode.Both:
					TranferBoth(simulation, transaction);
					break;
				case TransferMode.All:
					TransferAll(simulation, transaction);
					break;
				case TransferMode.Pick:
					TransferPick(simulation, transaction);
					break;
				case TransferMode.Function:
					TransferFunction(simulation, transaction);
					break;
				case TransferMode.Parameter:
					TransferParameter(simulation, transaction);
					break;
				case TransferMode.Subroutine:
					TransferSubroutine(simulation, transaction);
					break;
				case TransferMode.Simultaneous:
					TransferSimultaneous(simulation, transaction);
					break;
				default:
					throw new NotImplementedException();

			}
		}

		private void TransferSimultaneous(Simulation simulation, Transaction transaction)
		{
			if (transaction.Delayed)
			{
				transaction.Delayed = false;
				transaction.NextBlock = simulation.Model.Statements.Labels[SecondaryDestination(simulation.StandardAttributes)];
			}
			else
				transaction.NextBlock = simulation.Model.Statements.Labels[PrimaryDestination(simulation.StandardAttributes)];
		}

		// Работает иначе, чем в GPSS
		private void TransferParameter(Simulation simulation, Transaction transaction)
		{
			string nameBase = transaction.Parameters[PrimaryDestination(simulation.StandardAttributes)];
			string destination = string.Concat(nameBase, SecondaryDestination(simulation.StandardAttributes));
			transaction.NextBlock = simulation.Model.Statements.Labels[destination];
		}

		// Работает иначе, чем в GPSS
		private void TransferSubroutine(Simulation simulation, Transaction transaction)
		{
			var statements = simulation.Model.Statements;
			int index = statements.Blocks.IndexOf(this);
			string label = statements.Labels.FirstOrDefault(kvp => kvp.Value == index).Key;
			if (label == null)
				throw new ModelStructureException(
					"Attempt to Transfer Transaction in Parameter Mode via unnamed Transfer Block.",
					transaction.CurrentBlock);

			transaction.SetParameter(SecondaryDestination(simulation.StandardAttributes), label);
			transaction.NextBlock = statements.Labels[PrimaryDestination(simulation.StandardAttributes)];
		}

		// Работает иначе, чем в GPSS
		private void TransferFunction(Simulation simulation, Transaction transaction)
		{
			var function = simulation.Model.Calculations.Functions[PrimaryDestination(simulation.StandardAttributes)];
			function.Calculate(simulation.StandardAttributes);

			int value = (int)Math.Round(function.Result);
			string destination = string.Concat(SecondaryDestination(simulation.StandardAttributes), value.ToString());

			transaction.NextBlock = simulation.Model.Statements.Labels[destination];
		}

		private void TransferPick(Simulation simulation, Transaction transaction)
		{
			double roll = simulation.Model.Calculations.DefaultRandomGenerator.StandardUniform();
			if (roll < 0.5)
				transaction.NextBlock = simulation.Model.Statements.Labels[PrimaryDestination(simulation.StandardAttributes)];
			else
				transaction.NextBlock = simulation.Model.Statements.Labels[SecondaryDestination(simulation.StandardAttributes)];
		}

		private void TransferAll(Simulation simulation, Transaction transaction)
		{
			var statements = simulation.Model.Statements;
			int primaryBlockIndex = statements.Labels[PrimaryDestination(simulation.StandardAttributes)];
			int secondaryBlockIndex = statements.Labels[SecondaryDestination(simulation.StandardAttributes)];
			if (primaryBlockIndex >= secondaryBlockIndex)
				throw new ModelStructureException(
					"Attempt to perform tests for TRANSFER Block All Mode with " +
					"Primary Destination Block index higher or equal to Secondary Destination Block.",
					transaction.CurrentBlock);
			int increment = Increment(simulation.StandardAttributes);
			if (increment < 1)
				throw new ModelStructureException(
					"Attempt to perform tests for TRANSFER Block All Mode with " +
					"non-positive Increment value.",
					transaction.CurrentBlock);
			if ((secondaryBlockIndex - primaryBlockIndex) % increment != 0)
				throw new ModelStructureException(
					"Attempt to perform tests for TRANSFER Block All Mode with " +
					"invalid Increment value: loop not coming into Secondary Destination Block.",
					transaction.CurrentBlock);

			bool resolved = ResolveTransferAll(
				simulation, transaction,
				primaryBlockIndex, secondaryBlockIndex, increment);

			if (!resolved)
				AddToRetryChainsAll(
					simulation, transaction,
					primaryBlockIndex, secondaryBlockIndex, increment);
		}

		private void AddToRetryChainsAll(
			Simulation simulation,
			Transaction transaction,
			int primaryBlockIndex,
			int secondaryBlockIndex,
			int increment)
		{
			var statements = simulation.Model.Statements;
			for (int index = primaryBlockIndex; index <= secondaryBlockIndex; index += increment)
				statements.Blocks[index].AddRetry(simulation, index);

			transaction.State = TransactionState.Suspended;
			simulation.Scheduler.CurrentEvents.Remove(transaction);
		}

		private void AddToRetryChainsBoth(
			Simulation simulation,
			Transaction transaction,
			int primaryBlockIndex,
			int secondaryBlockIndex)
		{
			var statements = simulation.Model.Statements;
			statements.Blocks[primaryBlockIndex].AddRetry(simulation, primaryBlockIndex);
			statements.Blocks[secondaryBlockIndex].AddRetry(simulation, secondaryBlockIndex);

			transaction.State = TransactionState.Suspended;
			simulation.Scheduler.CurrentEvents.Remove(transaction);
		}

		public static bool ResolveTransferAll(
			Simulation simulation,
			Transaction transaction,
			int primaryBlockIndex,
			int secondaryBlockIndex,
			int increment)
		{
			var statements = simulation.Model.Statements;

			for (int index = primaryBlockIndex; index <= secondaryBlockIndex; index += increment)
				if (statements.Blocks[index].CanEnter(simulation))
				{
					transaction.NextBlock = index;
					return true;
				}
			return false;
		}

		private void TranferBoth(Simulation simulation, Transaction transaction)
		{
			var statements = simulation.Model.Statements;
			int primaryBlockIndex = statements.Labels[PrimaryDestination(simulation.StandardAttributes)];
			int secondaryBlockIndex = statements.Labels[SecondaryDestination(simulation.StandardAttributes)];

			bool resolved = ResolveTransferBoth(simulation, transaction, primaryBlockIndex, secondaryBlockIndex);
			if (!resolved)
				AddToRetryChainsBoth(simulation, transaction, primaryBlockIndex, secondaryBlockIndex);
		}

		public static bool ResolveTransferBoth(
			Simulation simulation,
			Transaction transaction,
			int primaryBlockIndex,
			int secondaryBlockIndex)
		{
			var statements = simulation.Model.Statements;
			if (statements.Blocks[primaryBlockIndex].CanEnter(simulation))
				transaction.NextBlock = primaryBlockIndex;
			else if (statements.Blocks[secondaryBlockIndex].CanEnter(simulation))
				transaction.NextBlock = secondaryBlockIndex;
			else
			{
				transaction.NextBlock = transaction.CurrentBlock;
				return false;
			}

			return true;
		}

		private void TransferFractional(Simulation simulation, Transaction transaction)
		{
			double fraction = Fraction(simulation.StandardAttributes);
			double roll = simulation.Model.Calculations.DefaultRandomGenerator.StandardUniform();
			if (roll <= fraction)
				transaction.NextBlock = simulation.Model.Statements.Labels[SecondaryDestination(simulation.StandardAttributes)];
			else if (PrimaryDestination(simulation.StandardAttributes) != null)
				transaction.NextBlock = simulation.Model.Statements.Labels[PrimaryDestination(simulation.StandardAttributes)];
		}

		private void TransferUnconditional(Simulation simulation, Transaction transaction)
		{
			transaction.NextBlock = simulation.Model.Statements.Labels[PrimaryDestination(simulation.StandardAttributes)];
		}

		public override Block Clone() => new Transfer
		{
			Mode = Mode,
			Fraction = Fraction,
			PrimaryDestination = PrimaryDestination,
			SecondaryDestination = SecondaryDestination,
			Increment = Increment,

			EntryCount = EntryCount,
			TransactionsCount = TransactionsCount,
		};
	}
}
