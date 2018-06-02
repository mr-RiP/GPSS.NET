using GPSS.StandardAttributes;
using GPSS.Entities.General;

namespace GPSS.SimulationParts
{
    internal class SystemCounters : ISystemAttributes
    {
        public int GenerationCount { get; set; }

        public double TerminationCount { get; set; }

        public double AbsoluteClock { get; set; }

        public double RelativeClock { get; set; }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateClock(double timeIncrement)
        {
            AbsoluteClock += timeIncrement;
            RelativeClock += timeIncrement;
        }
    }
}
