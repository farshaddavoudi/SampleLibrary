using System;
using System.Net;

namespace ATA.Library.Shared.Service.Exceptions
{
    public class AppException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public ApiResultStatusCodeEnum ApiStatusCodeEnum { get; set; }
        public object? AdditionalData { get; set; }

        public AppException()
            : this(ApiResultStatusCodeEnum.ServerError)
        {
        }

        public AppException(ApiResultStatusCodeEnum statusCodeEnum)
            : this(statusCodeEnum, null)
        {
        }

        public AppException(string message)
            : this(ApiResultStatusCodeEnum.ServerError, message)
        {
        }

        public AppException(ApiResultStatusCodeEnum statusCodeEnum, string? message)
            : this(statusCodeEnum, message, HttpStatusCode.InternalServerError)
        {
        }

        public AppException(string message, object additionalData)
            : this(ApiResultStatusCodeEnum.ServerError, message, additionalData)
        {
        }

        public AppException(ApiResultStatusCodeEnum statusCodeEnum, object additionalData)
            : this(statusCodeEnum, null, additionalData)
        {
        }

        public AppException(ApiResultStatusCodeEnum statusCodeEnum, string? message, object additionalData)
            : this(statusCodeEnum, message, HttpStatusCode.InternalServerError, additionalData)
        {
        }

        public AppException(ApiResultStatusCodeEnum statusCodeEnum, string? message, HttpStatusCode httpStatusCode)
            : this(statusCodeEnum, message, httpStatusCode, null)
        {
        }

        public AppException(ApiResultStatusCodeEnum statusCodeEnum, string? message, HttpStatusCode httpStatusCode, object additionalData)
            : this(statusCodeEnum, message, httpStatusCode, null, additionalData)
        {
        }

        public AppException(string message, Exception exception)
            : this(ApiResultStatusCodeEnum.ServerError, message, exception)
        {
        }

        public AppException(string message, Exception exception, object additionalData)
            : this(ApiResultStatusCodeEnum.ServerError, message, exception, additionalData)
        {
        }

        public AppException(ApiResultStatusCodeEnum statusCodeEnum, string message, Exception exception)
            : this(statusCodeEnum, message, HttpStatusCode.InternalServerError, exception)
        {
        }

        public AppException(ApiResultStatusCodeEnum statusCodeEnum, string message, Exception exception, object additionalData)
            : this(statusCodeEnum, message, HttpStatusCode.InternalServerError, exception, additionalData)
        {
        }

        public AppException(ApiResultStatusCodeEnum statusCodeEnum, string? message, HttpStatusCode httpStatusCode, Exception? exception)
            : this(statusCodeEnum, message, httpStatusCode, exception, null)
        {
        }

        public AppException(ApiResultStatusCodeEnum statusCodeEnum, string? message, HttpStatusCode httpStatusCode, Exception? exception, object? additionalData)
            : base(message, exception)
        {
            ApiStatusCodeEnum = statusCodeEnum;
            HttpStatusCode = httpStatusCode;
            AdditionalData = additionalData;
        }
    }
}