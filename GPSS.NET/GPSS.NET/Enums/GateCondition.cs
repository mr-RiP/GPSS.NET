namespace GPSS.Enums
{
    // http://www.minutemansoftware.com/reference/r7.htm#GATE
    public enum GateCondition
    {
        None = 0,

        FacilityAvailable,
        FacilityUnavailable,
        FacilityInterrupted,
        FacilityNotInterrupted,
        FacilityIdle,
        FacilityBusy,

        StorageEmpty,
        StorageFull,
        StorageNotEmpty,
        StorageNotFull,
        StorageAvailable,
        StorageUnavailable,

        LogicswitchSet,
        LogicswitchReset,

        Match,
        NotMatch,
    }
}

