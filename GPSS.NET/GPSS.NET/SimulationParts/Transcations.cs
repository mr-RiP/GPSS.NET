using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace GPSS.SimulationParts
{
    internal class Transcations
    {
        public ActiveTransaction ActiveTransaction { get; private set; } = new ActiveTransaction();

        public List<Transaction> Transactions { get; private set; } = new List<Transaction>();

        public int CurrentNumber { get; set; }

        public int TerminationCount { get; set; }
    }
}
