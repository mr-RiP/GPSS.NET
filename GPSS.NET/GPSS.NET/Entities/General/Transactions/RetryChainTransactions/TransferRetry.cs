using GPSS.Entities.General.Blocks;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General.Transactions.RetryChainTransactions
{
    internal class TransferRetry : RetryChainTransaction
    {
        public TransferRetry(
            Transaction innerTransaction,
            int primaryBlockIndex,
            int secondaryBlockIndex,
            int increment) : base(innerTransaction)
        {
            PrimaryBlockIndex = primaryBlockIndex;
            SecondaryBlockIndex = secondaryBlockIndex;
            Increment = increment;
        }

        public int PrimaryBlockIndex { get; private set; }
        public int SecondaryBlockIndex { get; private set; }
        public int Increment { get; private set; }

        public bool AllMode { get => Increment > 0; }
        public bool BothMode { get => Increment < 1; }

        public override bool Resolve(Simulation simulation)
        {
            if (AllMode)
                return Transfer.ResolveTransferAll(
                    simulation, InnerTransaction, PrimaryBlockIndex, SecondaryBlockIndex, Increment);
            else
                return Transfer.ResolveTransferBoth(
                    simulation, InnerTransaction, PrimaryBlockIndex, SecondaryBlockIndex);
        }
    }
}
