using GPSS.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Exceptions
{
    public class StandardAttributeAccessException : ArgumentException
    {
        public StandardAttributeAccessException(string message, EntityTypes entityType) : base(message)
        {
            EntityType = entityType;
        }

        public StandardAttributeAccessException(string message, EntityTypes entityType, Exception innerException) : base(message, innerException)
        {
            EntityType = entityType;
        }

        public EntityTypes EntityType { get; private set; }
    }
}
