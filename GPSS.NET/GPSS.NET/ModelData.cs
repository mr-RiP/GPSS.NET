using System.Collections.Generic;

namespace GPSS
{
    internal class ModelData
    {
        public List<GpssBlock> Blocks { get; private set; }

        public List<GpssLabel> Labels { get; private set; }

        public List<GpssQueue> Queues { get; private set; }

        public List<GpssSavedValue> SavedValues { get; private set; }

        public List<GpssStorage> Storages { get; private set; }
    }
}
