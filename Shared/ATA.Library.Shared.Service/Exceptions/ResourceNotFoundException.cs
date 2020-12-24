using ATA.Library.Shared.Core.CoreEnums;
using System;

namespace ATA.Library.Shared.Service.Exceptions
{
    public class ResourceNotFoundException : AppException
    {
        public ResourceNotFoundException()
            : base(ApiResultStatusCode.NotFound)
        {
        }

        public ResourceNotFoundException(string message)
            : base(ApiResultStatusCode.NotFound, message)
        {
        }

        public ResourceNotFoundException(object additionalData)
            : base(ApiResultStatusCode.NotFound, additionalData)
        {
        }

        public ResourceNotFoundException(string message, object additionalData)
            : base(ApiResultStatusCode.NotFound, message, additionalData)
        {
        }

        public ResourceNotFoundException(string message, Exception exception)
            : base(ApiResultStatusCode.NotFound, message, exception)
        {
        }

        public ResourceNotFoundException(string message, Exception exception, object additionalData)
            : base(ApiResultStatusCode.NotFound, message, exception, additionalData)
        {
        }
    }
}