using GPSS.Entities.Resources;
using GPSS.Extensions;
using GPSS.SimulationParts;
using System;
using System.Collections.Generic;

namespace GPSS.ModelParts
{
    internal class Resources : ICloneable, IResetable
    {
        public Dictionary<string, Facility> Facilities { get; private set; } = new Dictionary<string, Facility>();

        public Dictionary<string, Logicswitch> Logicswitches { get; private set; } = new Dictionary<string, Logicswitch>();

        public Dictionary<string, Storage> Storages { get; private set; } = new Dictionary<string, Storage>();

        public Resources Clone() => new Resources
        {
            Facilities = Facilities.Clone(),
            Logicswitches = Logicswitches.Clone(),
            Storages = Storages.Clone(),
        };

        object ICloneable.Clone() => Clone();

        public void Calculate(TransactionScheduler scheduler)
        {
            foreach (var storage in Storages.Values)
                storage.UpdateUsageHistory(scheduler);

            foreach (var facility in Facilities.Values)
                facility.UpdateUsageHistory(scheduler);
        }

        public void Reset()
        {
            foreach (var storage in Storages.Values)
                storage.Reset();

            foreach (var facility in Facilities.Values)
                facility.Reset();
        }

        public Facility GetFacility(string name, Simulation simulation)
        {
            if (Facilities.ContainsKey(name))
                return Facilities[name];
            else
            {
                var facility = new Facility();
                Facilities.Add(name, facility);
                simulation.Scheduler.FacilityDelayChains.Add(name, facility.DelayChain);
                return facility;
            }
        }
    }
}
