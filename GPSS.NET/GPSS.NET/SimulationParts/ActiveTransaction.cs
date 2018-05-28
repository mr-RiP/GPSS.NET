using GPSS.Entities.General;
using GPSS.StandardAttributes;


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
            throw new System.NotImplementedException();
        }

        public dynamic Parameter(string parameterName)
        {
            throw new System.NotImplementedException();
        }

        public double TransitTime(string parameterName)
        {
            throw new System.NotImplementedException();
        }

        public double TransitTime()
        {
            throw new System.NotImplementedException();
        }
    }
}
