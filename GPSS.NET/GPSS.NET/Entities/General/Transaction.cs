using GPSS.Enums;
using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Entities.General
{
    internal class Transaction : ICloneable
    {
        public virtual int Number { get; set; }

        public virtual int Priority { get; set; }

        public virtual int Assembly { get; set; }

        public virtual double MarkTime { get; set; }

        public virtual bool Trace { get; set; }

        public virtual int CurrentBlock { get; set; }

        public virtual int NextBlock { get; set; }

        public virtual TransactionState State { get; set; }

        public virtual bool Preempted { get; set; }

        public virtual Dictionary<string, dynamic> Parameters { get; private set; } = new Dictionary<string, dynamic>();

        public virtual Transaction Clone() => new Transaction
        {
            Number = Number,
            Assembly = Assembly,
            Priority = Priority,
            MarkTime = MarkTime,
            CurrentBlock = CurrentBlock,
            NextBlock = NextBlock,
            State = State,
            Parameters = new Dictionary<string, dynamic>(Parameters),
        };

        object ICloneable.Clone() => Clone();

        public virtual Transaction InnerTransaction { get => null; }

        public Transaction GetOriginal()
        {
            Transaction transaction = this;
            while (transaction.InnerTransaction != null)
                transaction = transaction.InnerTransaction;
            return transaction;
        }

        public void SetParameter<T>(string name, T value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (Parameters.ContainsKey(name))
                Parameters[name] = value;
            else
                Parameters.Add(name, value);
        }
    }
}                      
