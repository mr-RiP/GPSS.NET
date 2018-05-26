using GPSS.Entities.Calculations;
using System.Collections.Generic;

namespace GPSS.Modeling
{
    internal class Calculations
    {
        // Генератор по-умолчанию - от времени
        public RandomGenerator DefaultRandomGenerator { get; } = new RandomGenerator();
        
        // Пользовательские генераторы c пользовательским seed
        public Dictionary<int, RandomGenerator> RandomGenerators { get; } = new Dictionary<int, RandomGenerator>();

        public Dictionary<string, Function> Functions { get; } = new Dictionary<string, Function>();

        public Dictionary<string, Matrix> Matrices { get; } = new Dictionary<string, Matrix>();

        public Dictionary<string, Savevalue> Savevalues { get; } = new Dictionary<string, Savevalue>();

        public Dictionary<string, Variable<int>> Variables { get; } = new Dictionary<string, Variable<int>>();

        public Dictionary<string, Variable<double>> FloatVariables { get; } = new Dictionary<string, Variable<double>>();

        public Dictionary<string, Variable<bool>> BoolVariables { get; } = new Dictionary<string, Variable<bool>>();
    }
}
