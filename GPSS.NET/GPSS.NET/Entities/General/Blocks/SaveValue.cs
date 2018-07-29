using GPSS.Exceptions;
using GPSS.Extensions;
using System;

namespace GPSS.Entities.General.Blocks
{
    internal sealed class SaveValue : Block
    {
        private SaveValue()
        {

        }

        public SaveValue(Func<IStandardAttributes, string> name, Func<IStandardAttributes, dynamic> value)
        {
            Name = name;
            Value = value;
        }

        public Func<IStandardAttributes, string> Name { get; private set; }

        public Func<IStandardAttributes, dynamic> Value { get; private set; }

        public override string TypeName => "SAVEVALUE";

        public override void EnterBlock(Simulation simulation)
        {
            try
            {
                base.EnterBlock(simulation);
                string name = Name(simulation.StandardAttributes);
                var value = Value(simulation.StandardAttributes);

                var savevalue = simulation.Model.Calculations.GetSavevalue(name, simulation.Scheduler);
                savevalue.Value = value;
                savevalue.TestRetryChain(simulation.Scheduler);
            }
            catch (ArgumentNullException error)
            {
                throw new ModelStructureException(
                    "Attempt to access Savevalue entity by null name.",
                    GetCurrentBlockIndex(simulation),
                    error);
            }
        }

        public override Block Clone()
        {
            throw new NotImplementedException();
        }
    }
}
