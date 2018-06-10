﻿using System;
using System.Collections.Generic;
using System.Text;
using GPSS.Entities.Calculations;
using GPSS.Enums;
using GPSS.Exceptions;
using GPSS.Extensions;
using GPSS.ModelParts;
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
            Statements statements = simulation.Model.Statements;
            int index = AccessDictionary(statements.Labels, blockName, EntityTypes.Block);
            return statements.Blocks[index];
        }

        public IVariableAttributes<bool> BoolVariable(string variableName)
        {
            return AccessVariable(simulation.Model.Calculations.BoolVariables,
                variableName, EntityTypes.BoolVariable);
        }

        public IFacilityAttributes Facility(string facilityName)
        {
            var facility = AccessDictionary(
                simulation.Model.Resources.Facilities, facilityName, EntityTypes.Facility);
            facility.UpdateUsageHistory(simulation.Scheduler);
            return facility;
        }

        public IVariableAttributes<double> FloatVariable(string variableName)
        {
            return AccessVariable(simulation.Model.Calculations.FloatVariables,
                variableName, EntityTypes.FloatVariable);
        }

        public IFunctionAttributes Function(string functionName)
        {
            var function = AccessDictionary(simulation.Model.Calculations.Functions,
                functionName, EntityTypes.Function);
            Calculate(function, functionName, EntityTypes.Function);
            return function;
        }

        public ILogicswitchAttributes Logicswitch(string logicSwitchName)
        {
            return AccessDictionary(simulation.Model.Resources.Logicswitches,
                logicSwitchName, EntityTypes.Logicswitch);
        }

        public IMatrixAttributes Matrix(string matrixName)
        {
            return AccessDictionary(simulation.Model.Calculations.Matrices,
                matrixName, EntityTypes.Matrix);
        }

        public INumericGroupAttributes NumericGroup(string numericGroupName)
        {
            return AccessDictionary(simulation.Model.Groups.NumericGroups,
                numericGroupName, EntityTypes.NumericGroup);
        }

        public IQueueAttributes Queue(string queueName)
        {
            return AccessDictionary(simulation.Model.Statistics.Queues,
                queueName, EntityTypes.Queue);
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
                saveValueName, EntityTypes.Savevalue);
        }

        public IStorageAttributes Storage(string storageName)
        {
            var storage = AccessDictionary(
                simulation.Model.Resources.Storages, storageName, EntityTypes.Storage);
            storage.UpdateUsageHistory(simulation.Scheduler);
            return storage;

        }

        public ITableAttributes Table(string tableName)
        {
            return AccessDictionary(simulation.Model.Statistics.Tables,
                tableName, EntityTypes.Table);
        }

        public ITransactionAttributes Transaction()
        {
            if (simulation.ActiveTransaction.IsSet())
                return simulation.ActiveTransaction;
            else
                throw new StandardAttributeAccessException("Active transaction does not set.",
                    EntityTypes.Transaction);
        }

        public ITransactionGroupAttributes TransactionGroup(string transactionGroupName)
        {
            return AccessDictionary(simulation.Model.Groups.TransactionGroups,
                transactionGroupName, EntityTypes.TransactionGroup);
        }

        public IUserchainAttributes Userchain(string userChainName)
        {
            return AccessDictionary(simulation.Model.Groups.Userchains,
                userChainName, EntityTypes.Userchain);
        }

        public IVariableAttributes<int> Variable(string variableName)
        {
            return AccessVariable(simulation.Model.Calculations.Variables,
                variableName, EntityTypes.Variable);
        }

        public ISystemAttributes System()
        {
            return simulation.Scheduler;
        }

        private T AccessDictionary<T>(Dictionary<string, T> dictionary, string name, EntityTypes entityType)
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

        private Variable<T> AccessVariable<T>(Dictionary<string, Variable<T>> dictionary, string name, EntityTypes entityType)
        {
            var variable = AccessDictionary(dictionary, name, entityType);
            Calculate(variable, name, entityType);
            return variable;
        }

        private void Calculate<T>(ICalculatable<T> target, string name, EntityTypes entityType)
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
