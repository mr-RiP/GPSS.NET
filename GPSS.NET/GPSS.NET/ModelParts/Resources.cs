using GPSS.Entities.Resources;
using GPSS.Extensions;
using System;
using System.Collections.Generic;

namespace GPSS.ModelParts
{
    internal class Resources : ICloneable
    {
        public Dictionary<string, Facility> Facilities { get; private set; } = new Dictionary<string, Facility>();

        public Dictionary<string, LogicSwitch> LogicSwitches { get; private set; } = new Dictionary<string, LogicSwitch>();

        public Dictionary<string, Storage> Storages { get; private set; } = new Dictionary<string, Storage>();

        public Resources Clone() => new Resources
        {
            Facilities = Facilities.Clone(),
            LogicSwitches = LogicSwitches.Clone(),
            Storages = Storages.Clone(),
        };

        object ICloneable.Clone() => Clone();
    }
}
