using GPSS.Entities.Calculations;
using GPSS.Extensions;
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

        public void Clear()
        {
            foreach (var matrix in Matrices.Values)
                matrix.Clear();

            foreach (var savevalue in Savevalues.Values)
                savevalue.Clear();

            foreach (var variable in Variables.Values)
                variable.Clear();

            foreach (var variable in FloatVariables.Values)
                variable.Clear();

            foreach (var variable in BoolVariables.Values)
                variable.Clear();
        }

        object ICloneable.Clone() => Clone();
    }
}
