using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Enums
{
    // http://www.minutemansoftware.com/reference/r7.htm#GATE
    public enum GateCondition
    {
        FacilityAvailable = 0,
        FacilityUnavailable,
        FacilityInterrupted,
        FacilityNotInterrupted,
        FacilityIdle,
        FacilityBusy,

        StorageEmpty = 100,
        StorageFull,
        StorageNotEmpty,
        StorageNotFull,
        StorageAvailable,
        StorageUnavailable,

        LogicswitchSet = 200,
        LogicswitchReset,

        Match = 300,
        NotMatch,
    }
}
