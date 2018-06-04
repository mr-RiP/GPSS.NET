using GPSS.Entities.General;
using GPSS.Enums;
using GPSS.Exceptions;
using GPSS.Extensions;
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
        public ActiveTransaction(Simulation simulation)
        {
            this.simulation = simulation;
        }

        private Simulation simulation;

        public Transaction Transaction { get; private set; }

        public int Assembly => Transaction.Assembly;

        public int Priority => Transaction.Priority;

        public int Number => Transaction.Number;

        public bool MatchAtBlock(string blockName)
        {
            var modelGeneral = simulation.Model.Statements;
            if (modelGeneral.Labels.ContainsKey(blockName))
            {
                int blockIndex = modelGeneral.Labels[blockName];
                var chains = simulation.Chains;
                return chains.CurrentEvents.Any(t => t.CurrentBlock == blockIndex) ||
                    chains.FutureEvents.Any(t => t.CurrentBlock == blockIndex) ||
                    chains.StorageDelayChains.Any(kvp => kvp.Value.Any(t => t.CurrentBlock == blockIndex)) ||
                    chains.FacilityDelayChains.Any(kvp => kvp.Value.Any(t => t.CurrentBlock == blockIndex)) ||
                    chains.FacilityPendingChains.Any(kvp => kvp.Value.Any(t => t.CurrentBlock == blockIndex)) ||
                    chains.UserChains.Any(kvp => kvp.Value.Any(t => t.CurrentBlock == blockIndex));
            }
            else
                throw new StandardAttributeAccessException("Block with given name does not exists.", EntityTypes.Transaction);
        }

        public dynamic Parameter(string parameterName)
        {
            if (Transaction.Parameters.ContainsKey(parameterName))
                return Transaction.Parameters[parameterName];
            else
                throw new StandardAttributeAccessException("Active transaction does not have parameter with given name.", EntityTypes.Transaction);
        }

        public double TransitTime(string parameterName)
        {
            throw new NotImplementedException();
        }

        public double TransitTime()
        {
            throw new NotImplementedException();
        }

        public bool IsSet()
        {
            return Transaction != null && Transaction.Chain == TransactionState.Active;
        }

        public void Reset()
        {
            Transaction = simulation.Chains.GetActiveTransaction();
            if (Transaction != null)
                Transaction.Chain = TransactionState.Active;
        }

        internal void RunNextBlock()
        {
            simulation.Model.Statements.Blocks[Transaction.NextBlock].Run(simulation);
        }

        public void Clear()
        {
            Transaction = null;
        }
    }
}
