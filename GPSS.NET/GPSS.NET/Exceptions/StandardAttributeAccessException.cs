using GPSS.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSS.Exceptions
{
    public class StandardAttributeAccessException : ArgumentException
    {
        public StandardAttributeAccessException(string message, EntityType entityType) : base(message)
        {
            EntityType = entityType;
        }

        public StandardAttributeAccessException(string message, EntityType entityType, Exception innerException) : base(message, innerException)
        {
            EntityType = entityType;
        }

        public EntityType EntityType { get; private set; }
    }
}
