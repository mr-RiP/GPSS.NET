using GPSS.Entities.Groups;
using GPSS.Extensions;
using System;
using System.Collections.Generic;

namespace GPSS.ModelParts
{
    internal class Groups : ICloneable
    {
        public Dictionary<string, NumericGroup> NumericGroups { get; private set; } = new Dictionary<string, NumericGroup>();

        public Dictionary<string, TransactionGroup> TransactionGroups { get; private set; } = new Dictionary<string, TransactionGroup>();

        public Dictionary<string, Userchain> Userchains { get; private set; } = new Dictionary<string, Userchain>();

        public Groups Clone() => new Groups
        {
            NumericGroups = NumericGroups.Clone(),
            TransactionGroups = TransactionGroups.Clone(),
            Userchains = Userchains.Clone(),
        };

        object ICloneable.Clone() => Clone();
    }
}
