using System;

namespace ATA.Library.Shared.Service.Exceptions
{
    public class DomainLogicException : AppException
    {
        public DomainLogicException()
            : base(ApiResultStatusCodeEnum.LogicError)
        {
        }

        public DomainLogicException(string message)
            : base(ApiResultStatusCodeEnum.LogicError, message)
        {
        }

        public DomainLogicException(object additionalData)
            : base(ApiResultStatusCodeEnum.LogicError, additionalData)
        {
        }

        public DomainLogicException(string message, object additionalData)
            : base(ApiResultStatusCodeEnum.LogicError, message, additionalData)
        {
        }

        public DomainLogicException(string message, Exception exception)
            : base(ApiResultStatusCodeEnum.LogicError, message, exception)
        {
        }

        public DomainLogicException(string message, Exception exception, object additionalData)
            : base(ApiResultStatusCodeEnum.LogicError, message, exception, additionalData)
        {
        }
    }
}
