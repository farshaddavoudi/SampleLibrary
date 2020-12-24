using System;

namespace ATA.Library.Shared.Service.Exceptions
{
    public class ResourceNotFoundException : AppException
    {
        public ResourceNotFoundException()
            : base(ApiResultStatusCodeEnum.NotFound)
        {
        }

        public ResourceNotFoundException(string message)
            : base(ApiResultStatusCodeEnum.NotFound, message)
        {
        }

        public ResourceNotFoundException(object additionalData)
            : base(ApiResultStatusCodeEnum.NotFound, additionalData)
        {
        }

        public ResourceNotFoundException(string message, object additionalData)
            : base(ApiResultStatusCodeEnum.NotFound, message, additionalData)
        {
        }

        public ResourceNotFoundException(string message, Exception exception)
            : base(ApiResultStatusCodeEnum.NotFound, message, exception)
        {
        }

        public ResourceNotFoundException(string message, Exception exception, object additionalData)
            : base(ApiResultStatusCodeEnum.NotFound, message, exception, additionalData)
        {
        }
    }
}