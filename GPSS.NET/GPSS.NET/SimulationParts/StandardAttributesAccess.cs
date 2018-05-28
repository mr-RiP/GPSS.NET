using System;
using System.Collections.Generic;
using System.Text;
using GPSS.StandardAttributes;

namespace GPSS.SimulationParts
{
    internal class StandardAttributesAccess : IStandardAttributes
    {
        public StandardAttributesAccess(Simulation simulation)
        {
            this.simulation = simulation;
        }

        private Simulation simulation;

        public IBlockAttributes Block(string blockName)
        {
            throw new NotImplementedException();
        }

        public IVariableAttributes<bool> BoolVariable(string variableName)
        {
            throw new NotImplementedException();
        }

        public IFacilityAttributes Facility(int facilityName)
        {
            throw new NotImplementedException();
        }

        public IVariableAttributes<double> FloatVariable(string variableName)
        {
            throw new NotImplementedException();
        }

        public IFunctionAttributes Function(string functionName)
        {
            throw new NotImplementedException();
        }

        public ILogicSwitchAttributes LogicSwitch(string logicSwitchName)
        {
            throw new NotImplementedException();
        }

        public IMatrixAttributes Matrix(string matrixName)
        {
            throw new NotImplementedException();
        }

        public INumericGroupAttributes NumericGroup(string numericGroupName)
        {
            throw new NotImplementedException();
        }

        public IQueueAttributes Queue(string queueName)
        {
            throw new NotImplementedException();
        }

        public IRandomGeneratorAttributes RandomNumberGenerator()
        {
            throw new NotImplementedException();
        }

        public IRandomGeneratorAttributes RandomNumberGenerator(int seed)
        {
            throw new NotImplementedException();
        }

        public ISavevalueAttributes Savevalue(string saveValueName)
        {
            throw new NotImplementedException();
        }

        public IStorageAttributes Storage(string storageName)
        {
            throw new NotImplementedException();
        }

        public ITableAttributes Table(string tableName)
        {
            throw new NotImplementedException();
        }

        public ITransactionAttributes Transaction()
        {
            throw new NotImplementedException();
        }

        public ITransactionGroupAttributes TransactionGroup(string transactionGroupName)
        {
            throw new NotImplementedException();
        }

        public IUserChainAttributes UserChain(string userChainName)
        {
            throw new NotImplementedException();
        }

        public IVariableAttributes<int> Variable(string variableName)
        {
            throw new NotImplementedException();
        }
    }
}
