using GPSS.Entities.Calculations;
using GPSS.Entities.Calculations.Functions;
using GPSS.Entities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPSS
{
    public partial class Model
    {
        /// <summary>
        /// VARIABLE Command.
        /// A VARIABLE Command defines an arithmetic Variable Entity.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="expression">Variable expression.</param>
        /// <returns>Model with added Variable Entity.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> and <paramref name="expression"/> must not be null.
        /// </exception>
        /// /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="name"/> must be unique for the Model Variables.
        /// </exception>
        public Model Variable(string name, Func<IStandardAttributes, int> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(name));

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (Calculations.Variables.ContainsKey(name))
                throw new ArgumentOutOfRangeException(nameof(name), "Variable with given name already exists in the Model.");
            else
                Calculations.Variables.Add(name, new Variable<int>(expression));

            return this;
        }

        /// <summary>
        /// BVARIABLE Command.
        /// A BVARIABLE Command defines a Boolean Variable Entity.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="expression">Variable expression.</param>
        /// <returns>Model with added BVariable Entity.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> and <paramref name="expression"/> must not be null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="name"/> must be unique for the Model BoolVariables.
        /// </exception>
        public Model BoolVariable(string name, Func<IStandardAttributes, bool> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(name));

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (Calculations.BoolVariables.ContainsKey(name))
                throw new ArgumentOutOfRangeException(nameof(name), "BoolVariable with given name already exists in the Model.");
            else
                Calculations.BoolVariables.Add(name, new Variable<bool>(expression));

            return this;
        }

        /// <summary>
        /// FVARIABLE Command.
        /// A FVARIABLE Command defines a Floating point Variable Entity.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="expression">Variable expression.</param>
        /// <returns>Model with added FVariable Entity.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> and <paramref name="expression"/> must not be null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="name"/> must be unique for the Model FloatVariables.
        /// </exception>
        public Model FloatVariable(string name, Func<IStandardAttributes, double> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(name));

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (Calculations.FloatVariables.ContainsKey(name))
                throw new ArgumentOutOfRangeException(nameof(name), "FloatVariable with given name already exists in the Model.");
            else
                Calculations.FloatVariables.Add(name, new Variable<double>(expression));

            return this;
        }

        /// <summary>
        /// FUNCTION Command with C type function.
        /// A FUNCTION Command defines the rules for a table lookup.
        /// Type C - "Continuous" valued Function. Performs a linear interpolation.
        /// </summary>
        /// <param name="name">Function name. Must be unique within the Model functions.</param>
        /// <param name="argument">Function argument.</param>
        /// <param name="values">Function argument-result values pairs.</param>
        /// <returns>Model with added Function Entity.</returns>
        /// <exception cref="ArgumentNullException">All arguments must not be null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="values"/> must not be empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="name"/> must be unique for the Model Function Entities.</exception>
        public Model ContinuousFunction(string name, Func<IStandardAttributes, double> argument,
            IEnumerable<KeyValuePair<double, double>> values)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (argument == null)
                throw new ArgumentNullException(nameof(argument));
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            if (!values.Any())
                throw new ArgumentOutOfRangeException(nameof(values), "Collection must not be empty.");

            if (Calculations.Functions.ContainsKey(name))
                throw new ArgumentOutOfRangeException(nameof(name), "Function with given name already exists in the Model.");
            else
                Calculations.Functions.Add(name, new ContinuousFunction(argument, values));

            return this;
        }

        /// <summary>
        /// FUNCTION Command with D type function.
        /// A FUNCTION Command defines the rules for a table lookup.
        /// Type  D - Discrete valued function. Each argument value or probability mass is assigned an numeric value.
        /// </summary>
        /// <param name="name">Function name. Must be unique within the Model functions.</param>
        /// <param name="argument">Function argument.</param>
        /// <param name="values">Function argument-result values pairs.</param>
        /// <returns>Model with added Function Entity.</returns>
        /// <exception cref="ArgumentNullException">All arguments must not be null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="values"/> must not be empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="name"/> must be unique for the Model Function Entities.</exception>
        public Model DiscreteFunction(string name, Func<IStandardAttributes, double> argument,
            IEnumerable<KeyValuePair<double, double>> values)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (argument == null)
                throw new ArgumentNullException(nameof(argument));
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            if (!values.Any())
                throw new ArgumentOutOfRangeException(nameof(values), "Collection must not be empty.");

            if (Calculations.Functions.ContainsKey(name))
                throw new ArgumentOutOfRangeException(nameof(name), "Function with given name already exists in the Model.");
            else
                Calculations.Functions.Add(name, new DiscreteFunction(argument, values));

            return this;
        }

        /// <summary>
        /// FUNCTION Command with E type function.
        /// A FUNCTION Command defines the rules for a table lookup.
        /// Type E - Discrete, "attribute valued" Function. Each argument value or probability mass is assigned an SNA to be evaluated.
        /// </summary>
        /// <param name="name">Function name. Must be unique within the Model functions.</param>
        /// <param name="argument">Function argument.</param>
        /// <param name="values">Function argument-result calculation function pairs.</param>
        /// <returns>Model with added Function Entity.</returns>
        /// <exception cref="ArgumentNullException">All arguments must not be null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="values"/> must not be empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="name"/> must be unique for the Model Function Entities.</exception>
        public Model DiscreteAttributeFunction(string name, Func<IStandardAttributes, double> argument,
            IEnumerable<KeyValuePair<double, Func<IStandardAttributes, double>>> values)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (argument == null)
                throw new ArgumentNullException(nameof(argument));
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            if (!values.Any())
                throw new ArgumentOutOfRangeException(nameof(values), "Collection must not be empty.");

            if (Calculations.Functions.ContainsKey(name))
                throw new ArgumentOutOfRangeException(nameof(name), "Function with given name already exists in the Model.");
            else
                Calculations.Functions.Add(name, new DiscreteAttributeFunction(argument, values));

            return this;
        }

        /// <summary>
        /// FUNCTION Command with L type function.
        /// A FUNCTION Command defines the rules for a table lookup.
        /// Type L - List valued Function. The argument value is used to determine the list position of the value to be returned.
        /// </summary>
        /// <param name="name">Function name. Must be unique within the Model functions.</param>
        /// <param name="argument">Function argument.</param>
        /// <param name="values">Function result values list.</param>
        /// <returns>Model with added Function Entity.</returns>
        /// <exception cref="ArgumentNullException">All arguments must not be null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="values"/> must not be empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="name"/> must be unique for the Model Function Entities.</exception>
        public Model ListFunction(string name, Func<IStandardAttributes, int> argument, IList<double> values)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (argument == null)
                throw new ArgumentNullException(nameof(argument));
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            if (!values.Any())
                throw new ArgumentOutOfRangeException(nameof(values), "Collection must not be empty.");

            if (Calculations.Functions.ContainsKey(name))
                throw new ArgumentOutOfRangeException(nameof(name), "Function with given name already exists in the Model.");
            else
                Calculations.Functions.Add(name, new ListFunction(argument, values));

            return this;
        }

        /// <summary>
        /// FUNCTION Command with M type function.
        /// A FUNCTION Command defines the rules for a table lookup.
        /// Type M - Attribute list valued function. 
        /// The argument value is used to determine the list position of the SNA to be evaluated and returned as the result.
        /// </summary>
        /// <param name="name">Function name. Must be unique within the Model functions.</param>
        /// <param name="argument">Function argument.</param>
        /// <param name="values">Function result calculation functions list.</param>
        /// <returns>Model with added Function Entity.</returns>
        /// <exception cref="ArgumentNullException">All arguments must not be null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="values"/> must not be empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="name"/> must be unique for the Model Function Entities.</exception>
        public Model AttributeListFunction(string name, Func<IStandardAttributes, int> argument,
            IList<Func<IStandardAttributes, double>> values)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (argument == null)
                throw new ArgumentNullException(nameof(argument));
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            if (!values.Any())
                throw new ArgumentOutOfRangeException(nameof(values), "Collection must not be empty.");

            if (Calculations.Functions.ContainsKey(name))
                throw new ArgumentOutOfRangeException(nameof(name), "Function with given name already exists in the Model.");
            else
                Calculations.Functions.Add(name, new AttributeListFunction(argument, values));

            return this;
        }

        /// <summary>
        /// STORAGE Command.
        /// A STORAGE Command defines a Storage Entity.
        /// </summary>
        /// <param name="name">Name of the Storage entity to be created.</param>
        /// <param name="totalCapacity">Total storage capacity of the Storage entity.</param>
        /// <returns>Model with added Storage Entity.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> can not be null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="totalCapacity"/> must have positive value.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="name"/> should be unique for the Model Storage Entities.</exception>
        public Model Storage(string name, int totalCapacity)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (totalCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(totalCapacity), "Argument value must be positive.");

            if (Resources.Storages.ContainsKey(name))
                throw new ArgumentOutOfRangeException(nameof(name), "Storage with given name already exists in the Model.");
            else
                Resources.Storages.Add(name, new Storage(totalCapacity));

            return this;
        }

        /// <summary>
        /// Labels following Block with given <paramref name="name"/>.
        /// Block can have many names but every name can address only one Block.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Model with added Label.</returns>
        public Model Label(string name)
        {
            
            return this;
        }
    }
}
