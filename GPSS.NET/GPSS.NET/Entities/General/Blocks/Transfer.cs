using GPSS.Enums;
using GPSS.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General.Blocks
{
    internal class Transfer : Block
    {
        public class Values : ICloneable
        {
            public TransferMode Mode { get; set; }
            public double Fraction { get; set; }
            public string PrimaryDestination { get; set; }
            public string SecondaryDestination { get; set; }
            public int Increment { get; set; }

            object ICloneable.Clone() => Clone();
            public Values Clone() => new Values
            {
                Mode = Mode,
                Fraction = Fraction,
                PrimaryDestination = PrimaryDestination,
                SecondaryDestination = SecondaryDestination,
                Increment = Increment,
            };
        }

        private Transfer()
        {

        }

        public Transfer(
            Func<IStandardAttributes, TransferMode> mode,
            Func<IStandardAttributes, string> primaryDestination,
            Func<IStandardAttributes, string> secondaryDestination,
            Func<IStandardAttributes, int> increment)
        {
            Mode = mode;
            PrimaryDestination = primaryDestination;
            SecondaryDestination = secondaryDestination;
            Increment = increment;
        }

        public Func<IStandardAttributes, TransferMode> Mode { get; private set; }
        public Func<IStandardAttributes, double> Fraction { get; private set; }
        public Func<IStandardAttributes, string> PrimaryDestination { get; private set; }
        public Func<IStandardAttributes, string> SecondaryDestination { get; private set; }
        public Func<IStandardAttributes, int> Increment { get; private set; }

        public Dictionary<Transaction, Values> RetryChain { get; private set; } = new Dictionary<Transaction, Values>();
        public override int RetryCount => RetryChain.Count;

        public override string TypeName => "TRANSFER";

        public override void EnterBlock(Simulation simulation)
        {
            base.EnterBlock(simulation);
        }

        public override Block Clone() => new Transfer
        {
            Mode = Mode,
            Fraction = Fraction,
            PrimaryDestination = PrimaryDestination,
            SecondaryDestination = SecondaryDestination,
            Increment = Increment,

            RetryChain = RetryChain.Clone(),

            EntryCount = EntryCount,
            TransactionsCount = TransactionsCount,
        };
    }
}
