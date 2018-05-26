using GPSS.Entities.Resources;
using System.Collections.Generic;

namespace GPSS.Modeling
{
    internal class Resources
    {
        public Dictionary<string, Facility> Facilities { get; } = new Dictionary<string, Facility>();

        public Dictionary<string, LogicSwitch> LogicSwitches { get; } = new Dictionary<string, LogicSwitch>();

        public Dictionary<string, Storage> Storages { get; } = new Dictionary<string, Storage>();
    }
}
