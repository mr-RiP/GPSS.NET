using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.StandardAttributes
{
    public interface ISystemAttributes
    {
        /// <summary>
        /// Remaining Termination Count. The count that is decremented by TERMINATE Blocks with a positive argument.
        /// </summary>
        /// <remarks>
        /// GPSS World TG1 SNA.
        /// </remarks>
        double TerminationCount { get; }

        /// <summary>
        /// Value of absolute system clock. Simulated time since last CLEAR.
        /// </summary>
        /// <remarks>
        /// GPSS World AC1 SNA.
        /// </remarks>
        double AbsoluteClock { get; }

        /// <summary>
        /// Value of relative system clock. Simulated time since last RESET.
        /// </summary>
        /// <remarks>
        /// GPSS World C1 SNA.
        /// </remarks>
        double RelativeClock { get; }
    }
}
