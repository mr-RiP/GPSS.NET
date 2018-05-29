using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace GPSS.SimulationParts
{
    // Томашевский 4.22. Управление процессом моделирования в системе GPSS стр. 178 (176 в доке)
    internal class Events
    {
        public ActiveTransaction ActiveTransaction { get; private set; } = new ActiveTransaction();

        public List<Transaction> FutureEvents { get; private set; } = new List<Transaction>();

        public List<Transaction> CurrentEvents { get; private set; } = new List<Transaction>();

        public List<Transaction> InterruptedEvents { get; private set; } = new List<Transaction>();

        public int TerminationCount { get; set; }

        public int GenerationCount { get; set; }

        public double Time { get; set; }
    }
}
