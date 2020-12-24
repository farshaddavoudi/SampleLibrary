using ATA.Library.Shared.Core.CoreEnums;
using System;

namespace ATA.Library.Shared.Service.Exceptions
{
    public class DomainLogicException : AppException
    {
        public DomainLogicException()
            : base(ApiResultStatusCode.LogicError)
        {
        }

        public DomainLogicException(string message)
            : base(ApiResultStatusCode.LogicError, message)
        {
        }

        public DomainLogicException(object additionalData)
            : base(ApiResultStatusCode.LogicError, additionalData)
        {
        }

        public DomainLogicException(string message, object additionalData)
            : base(ApiResultStatusCode.LogicError, message, additionalData)
        {
        }

        public DomainLogicException(string message, Exception exception)
            : base(ApiResultStatusCode.LogicError, message, exception)
        {
        }

        public DomainLogicException(string message, Exception exception, object additionalData)
            : base(ApiResultStatusCode.LogicError, message, exception, additionalData)
        {
        }
    }
}
