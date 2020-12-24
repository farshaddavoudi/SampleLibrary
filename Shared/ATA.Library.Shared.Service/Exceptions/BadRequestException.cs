using System;

namespace ATA.Library.Shared.Service.Exceptions
{
    public class BadRequestException : AppException
    {
        public BadRequestException()
            : base(ApiResultStatusCodeEnum.BadRequest)
        {
        }

        public BadRequestException(string message)
            : base(ApiResultStatusCodeEnum.BadRequest, message)
        {
        }

        public BadRequestException(object additionalData)
            : base(ApiResultStatusCodeEnum.BadRequest, additionalData)
        {
        }

        public BadRequestException(string message, object additionalData)
            : base(ApiResultStatusCodeEnum.BadRequest, message, additionalData)
        {
        }

        public BadRequestException(string message, Exception exception)
            : base(ApiResultStatusCodeEnum.BadRequest, message, exception)
        {
        }

        public BadRequestException(string message, Exception exception, object additionalData)
            : base(ApiResultStatusCodeEnum.BadRequest, message, exception, additionalData)
        {
        }
    }
}
