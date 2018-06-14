using GPSS.ModelParts;
using System;

namespace GPSS
{
    public partial class Model : ICloneable
    {
        public Model()
        {
            Clear();
        }

        public Simulation CreateSimulation()
        {
            return new Simulation(this);
        }   

        internal Statements Statements { get; private set; }
        internal Calculations Calculations { get; private set; }
        internal Groups Groups { get; private set; }
        internal Resources Resources { get; private set; }
        internal Statistics Statistics { get; private set; }

        public Model Clone() => new Model()
        {
            Statements = Statements.Clone(),
            Calculations = Calculations.Clone(),
            Groups = Groups.Clone(),
            Resources = Resources.Clone(),
            Statistics = Statistics.Clone(),
        };

        object ICloneable.Clone() => Clone();

        /// <summary>
        /// Labels following Block with given <paramref name="name"/>.
        /// Block can have many names but every name can address only one Block.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Model with added Label, waiting for next given Block.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> must not be null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="name"/> must be unique within the Model Label names.</exception>
        public Model Label(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (Statements.Labels.ContainsKey(name))
                throw new ArgumentOutOfRangeException(nameof(name), "Label with given name already exists in the Model.");

            Statements.Labels.Add(name, Statements.Blocks.Count);
            return this;
        }

        public void Clear()
        {
            Statements = new Statements();
            Calculations = new Calculations();
            Groups = new Groups();
            Resources = new Resources();
            Statistics = new Statistics();
        }
    }
}
