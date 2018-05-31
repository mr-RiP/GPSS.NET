using GPSS.Enums;
using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General
{
    internal class Transaction : ICloneable
    {
        public int Number { get; set; }

        public int Priority { get; set; }

        public int Assembly { get; set; }

        public double TimeIncrement { get; set; }

        public double MarkTime { get; set; }

        public bool Trace { get; set; }

        public int CurrentBlock { get; set; }

        public int NextBlock { get; set; }

        public TransactionState Chain { get; set; }

        public bool Preempted { get; set; }

        public Transaction Clone() => new Transaction
        {
            Number = Number,
            Assembly = Assembly,
            Priority = Priority,
            TimeIncrement = TimeIncrement,
            MarkTime = MarkTime,
            CurrentBlock = CurrentBlock,
            NextBlock = NextBlock,
            Chain = Chain,
        };

        object ICloneable.Clone() => Clone();
    }
}                      
