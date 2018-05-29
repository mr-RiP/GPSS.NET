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

        public int Assembly { get; set; }

        public int Priority { get; set; }

        public double CurrentTime { get; set; }

        public double TransitTime { get; set; }

        public double CreationTime { get; set; }

        public int CurrentBlockIndex { get; set; }

        public int TargetBlockIndex { get; set; }

        public TransactionState Chain { get; set; }

        public Transaction Clone() => new Transaction
        {
            Number = Number,
            Assembly = Assembly,
            Priority = Priority,
            CurrentTime = CurrentTime,
            TransitTime = TransitTime,
            CreationTime = CreationTime,
            CurrentBlockIndex = CurrentBlockIndex,
            TargetBlockIndex = TargetBlockIndex,
            Chain = Chain,
        };

        object ICloneable.Clone() => Clone();
    }
}                      
