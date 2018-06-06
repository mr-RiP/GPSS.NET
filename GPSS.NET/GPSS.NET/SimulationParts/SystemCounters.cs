using GPSS.StandardAttributes;
using GPSS.Extensions;

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
            GenerationCount = 0;
            TerminationCount = 0.0;
            AbsoluteClock = 0.0;
            RelativeClock = 0.0;
        }

        public void Reset()
        {
            RelativeClock = 0.0;
        }

        public void UpdateClock(double currentTime)
        {
            AbsoluteClock += currentTime - RelativeClock;
            RelativeClock = currentTime;
        }
    }
}
