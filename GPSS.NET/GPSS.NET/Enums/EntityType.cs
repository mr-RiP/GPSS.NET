using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Enums
{
    public enum EntityType
    {
        // General
        Block,
        Transaction,

        // Calculations
        Function,
        Matrix,
        RandomGenerator,
        Savevalue,
        BoolVariable,
        FloatVariable,
        Variable,

        // Groups
        NumericGroup,
        TransactionGroup,
        Userchain,

        // Resources
        Facility,
        Logicswitch,
        Storage,

        // Statistics
        Queue,
        Table
    }
}
