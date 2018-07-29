using GPSS.Entities.Resources;
using GPSS.Enums;
using System;

namespace GPSS.Extensions
{
    internal static class GateConditionExtensions
    {
        public static EntityType GetEntityType(this GateCondition condition)
        {
            switch (condition)
            {
                case GateCondition.FacilityAvailable:
                case GateCondition.FacilityBusy:
                case GateCondition.FacilityIdle:
                case GateCondition.FacilityInterrupted:
                case GateCondition.FacilityNotInterrupted:
                case GateCondition.FacilityUnavailable:
                    return EntityType.Facility;

                case GateCondition.LogicswitchReset:
                case GateCondition.LogicswitchSet:
                    return EntityType.Logicswitch;

                case GateCondition.Match:
                case GateCondition.NotMatch:
                    return EntityType.Block;

                case GateCondition.StorageAvailable:
                case GateCondition.StorageEmpty:
                case GateCondition.StorageFull:
                case GateCondition.StorageNotEmpty:
                case GateCondition.StorageNotFull:
                case GateCondition.StorageUnavailable:
                    return EntityType.Storage;

                default:
                    throw new NotImplementedException();
            }
        }

        public static Func<bool> GetTest(this GateCondition condition, object testObject)
        {
            switch (condition)
            {
                case GateCondition.FacilityAvailable:
                    return (() => ((Facility)testObject).Available);
                case GateCondition.FacilityBusy:
                    return (() => ((Facility)testObject).Busy);
                case GateCondition.FacilityIdle:
                    return (() => ((Facility)testObject).Idle);
                case GateCondition.FacilityInterrupted:
                    return (() => ((Facility)testObject).Interrupted);
                case GateCondition.FacilityNotInterrupted:
                    return (() => !((Facility)testObject).Interrupted);
                case GateCondition.FacilityUnavailable:
                    return (() => !((Facility)testObject).Available);

                case GateCondition.StorageAvailable:
                    return (() => ((Storage)testObject).Available);
                case GateCondition.StorageEmpty:
                    return (() => ((Storage)testObject).Empty);
                case GateCondition.StorageFull:
                    return (() => ((Storage)testObject).Full);
                case GateCondition.StorageNotEmpty:
                    return (() => !((Storage)testObject).Empty);
                case GateCondition.StorageNotFull:
                    return (() => !((Storage)testObject).Full);
                case GateCondition.StorageUnavailable:
                    return (() => !((Storage)testObject).Available);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
