using GPSS.Entities.General;
using GPSS.Enums;
using GPSS.StandardAttributes;
using System;
using System.Linq;

namespace GPSS.SimulationParts
{
    // Транзакт в чистом виде не содержит в себе всей информации, которая доступна по его аттрибутам
    // Поэтому, используем этот адаптер
    // TODO - добавить в него необходимые элементы
    internal class ActiveTransaction : ITransactionAttributes
    {
        public Transaction Transaction { get; set; }

        public int Assembly => Transaction.Assembly;

        public int Priority => Transaction.Priority;

        public int Number => Transaction.Number;

        public bool MatchAtBlock(string blockName)
        {
            throw new NotImplementedException();
        }

        public dynamic Parameter(string parameterName)
        {
            throw new NotImplementedException();
        }

        public double TransitTime(string parameterName)
        {
            throw new NotImplementedException();
        }

        public double TransitTime()
        {
            throw new NotImplementedException();
        }

        public bool Set()
        {
            return Transaction != null && Transaction.Chain == TransactionState.Active;
        }

        public bool Set(TransactionChains chains)
        {
            if (!Set())
            {
                if (!chains.CurrentEvents.Any())
                    chains.UpdateCurrentEvents();

                Transaction = chains.ActiveTransaction();
                if (Transaction != null)
                {
                    Transaction.Chain = TransactionState.Active;
                    return true;
                }
                else
                    return false;
            }
            else
                return true;
        }

        internal void RunNextBlock(Simulation simulation)
        {
            simulation.Model.General.Blocks[Transaction.NextBlock].Run(simulation);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
