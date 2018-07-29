using GPSS.Entities.Calculations;
using GPSS.Enums;
using GPSS.Exceptions;
using GPSS.Extensions;
using GPSS.ModelParts;
using GPSS.StandardAttributes;
using System;
using System.Collections.Generic;

namespace GPSS.SimulationParts
{
    internal sealed class StandardAttributesAccess : IStandardAttributes
    {
        public StandardAttributesAccess(Simulation simulation)
        {
            this.simulation = simulation;
        }

        private Simulation simulation;

        public IBlockAttributes Block(string blockName)
        {
            Statements statements = simulation.Model.Statements;
            int index = AccessDictionary(statements.Labels, blockName, EntityType.Block);
            return statements.Blocks[index];
        }

        public IVariableAttributes<bool> BoolVariable(string variableName)
        {
            return AccessVariable(simulation.Model.Calculations.BoolVariables,
                variableName, EntityType.BoolVariable);
        }

        public IFacilityAttributes Facility(string facilityName)
        {
            var facility = AccessDictionary(
                simulation.Model.Resources.Facilities, facilityName, EntityType.Facility);
            facility.UpdateUsageHistory(simulation.Scheduler);
            return facility;
        }

        public IVariableAttributes<double> FloatVariable(string variableName)
        {
            return AccessVariable(simulation.Model.Calculations.FloatVariables,
                variableName, EntityType.FloatVariable);
        }

        public IFunctionAttributes Function(string functionName)
        {
            var function = AccessDictionary(simulation.Model.Calculations.Functions,
                functionName, EntityType.Function);
            Calculate(function, functionName, EntityType.Function);
            return function;
        }

        public ILogicswitchAttributes Logicswitch(string logicSwitchName)
        {
            return AccessDictionary(simulation.Model.Resources.Logicswitches,
                logicSwitchName, EntityType.Logicswitch);
        }

        public IMatrixAttributes Matrix(string matrixName)
        {
            return AccessDictionary(simulation.Model.Calculations.Matrices,
                matrixName, EntityType.Matrix);
        }

        public INumericGroupAttributes NumericGroup(string numericGroupName)
        {
            return AccessDictionary(simulation.Model.Groups.NumericGroups,
                numericGroupName, EntityType.NumericGroup);
        }

        public IQueueAttributes Queue(string queueName)
        {
            return AccessDictionary(simulation.Model.Statistics.Queues,
                queueName, EntityType.Queue);
        }

        public IRandomNumberGeneratorAttributes RandomNumberGenerator()
        {
            return simulation.Model.Calculations.DefaultRandomGenerator;
        }

        public IRandomNumberGeneratorAttributes RandomNumberGenerator(int seed)
        {
            var dictionary = simulation.Model.Calculations.RandomGenerators;
            if (dictionary.ContainsKey(seed))
                return dictionary[seed];
            else
            {
                var generator = new RandomNumberGenerator(seed);
                dictionary.Add(seed, generator);
                return generator;
            }
        }

        public ISavevalueAttributes Savevalue(string saveValueName)
        {
            return AccessDictionary(simulation.Model.Calculations.Savevalues,
                saveValueName, EntityType.Savevalue);
        }

        public IStorageAttributes Storage(string storageName)
        {
            var storage = AccessDictionary(
                simulation.Model.Resources.Storages, storageName, EntityType.Storage);
            storage.UpdateUsageHistory(simulation.Scheduler);
            return storage;

        }

        public ITableAttributes Table(string tableName)
        {
            return AccessDictionary(simulation.Model.Statistics.Tables,
                tableName, EntityType.Table);
        }

        public ITransactionAttributes Transaction()
        {
            if (simulation.ActiveTransaction.IsSet())
                return simulation.ActiveTransaction;
            else
                throw new StandardAttributeAccessException("Active transaction does not set.",
                    EntityType.Transaction);
        }

        public ITransactionGroupAttributes TransactionGroup(string transactionGroupName)
        {
            return AccessDictionary(simulation.Model.Groups.TransactionGroups,
                transactionGroupName, EntityType.TransactionGroup);
        }

        public IUserchainAttributes Userchain(string userChainName)
        {
            return AccessDictionary(simulation.Model.Groups.Userchains,
                userChainName, EntityType.Userchain);
        }

        public IVariableAttributes<int> Variable(string variableName)
        {
            return AccessVariable(simulation.Model.Calculations.Variables,
                variableName, EntityType.Variable);
        }

        public ISystemAttributes System()
        {
            return simulation.Scheduler;
        }

        private T AccessDictionary<T>(Dictionary<string, T> dictionary, string name, EntityType entityType)
        {
            if (name == null)
                throw new StandardAttributeAccessException(
                    "Can not access Standard Attributes using null name.",
                    entityType,
                    new ArgumentNullException());

            if (dictionary.ContainsKey(name))
                return dictionary[name];
            else
                throw new StandardAttributeAccessException(
                    entityType.ToString() + " by name \"" + name + "\" does not exists.",
                    entityType,
                    new ArgumentOutOfRangeException());
        }

        private Variable<T> AccessVariable<T>(Dictionary<string, Variable<T>> dictionary, string name, EntityType entityType)
            where T : struct
        {
            var variable = AccessDictionary(dictionary, name, entityType);
            Calculate(variable, name, entityType);
            return variable;
        }

        private void Calculate<T>(ICalculatable<T> target, string name, EntityType entityType)
        {
            try
            {
                target.Calculate(this);
            }
            catch (Exception error)
            {
                throw new StandardAttributeAccessException(
                    entityType.ToString() + " by name \"" + name + "\" could not been calculated.",
                    entityType,
                    error);
            }
        }
    }
}
