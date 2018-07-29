using GPSS.Entities.Calculations;
using GPSS.Extensions;
using GPSS.SimulationParts;
using System;
using System.Collections.Generic;

namespace GPSS.ModelParts
{
    internal class Calculations : ICloneable
    {
        // Генератор по-умолчанию - от времени
        public RandomNumberGenerator DefaultRandomGenerator { get; private set; } = new RandomNumberGenerator();

        // Пользовательские генераторы c пользовательским seed
        public Dictionary<int, RandomNumberGenerator> RandomGenerators { get; private set; } = new Dictionary<int, RandomNumberGenerator>();

        public Dictionary<string, Function> Functions { get; private set; } = new Dictionary<string, Function>();

        public Dictionary<string, Matrix> Matrices { get; private set; } = new Dictionary<string, Matrix>();

        public Dictionary<string, Savevalue> Savevalues { get; private set; } = new Dictionary<string, Savevalue>();

        public Dictionary<string, Variable<int>> Variables { get; private set; } = new Dictionary<string, Variable<int>>();

        public Dictionary<string, Variable<double>> FloatVariables { get; private set; } = new Dictionary<string, Variable<double>>();

        public Dictionary<string, Variable<bool>> BoolVariables { get; private set; } = new Dictionary<string, Variable<bool>>();

        public Calculations Clone() => new Calculations
        {
            DefaultRandomGenerator = DefaultRandomGenerator,
            RandomGenerators = RandomGenerators.Clone(),
            Functions = Functions.Clone(),
            Matrices = Matrices.Clone(),
            Savevalues = Savevalues.Clone(),
            Variables = Variables.Clone(),
            FloatVariables = FloatVariables.Clone(),
            BoolVariables = BoolVariables.Clone(),
        };

        object ICloneable.Clone() => Clone();

        public Savevalue GetSavevalue(string name, TransactionScheduler scheduler)
        {
            if (Savevalues.TryGetValue(name, out Savevalue result))
            {
                return result;
            }
            else
            {
                var sv = new Savevalue();
                scheduler.RetryChains.Add(sv, sv.RetryChain);
                return sv;
            }
        }
    }
}
