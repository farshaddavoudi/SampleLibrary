using ATA.Library.Shared.Service.Exceptions;
using ATA.Library.Shared.Service.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace ATA.Library.Shared.Dto
{
    public partial class ApiResult
    {
        public ApiResult(bool isSuccess, ApiResultStatusCodeEnum customStatusCode, string? message = null)
        {
            IsSuccess = isSuccess;
            CustomStatusCode = customStatusCode;
            Message = message ?? customStatusCode.ToDisplayName()!;
        }

        #region Implicit Operators
        public static implicit operator ApiResult(OkResult result)
        {
            return new ApiResult(true, ApiResultStatusCodeEnum.Success);
        }

        public static implicit operator ApiResult(BadRequestResult result)
        {
            return new ApiResult(false, ApiResultStatusCodeEnum.BadRequest);
        }

        public static implicit operator ApiResult(BadRequestObjectResult result)
        {
            var message = result.Value.ToString();
            if (result.Value is SerializableError errors)
            {
                var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
                message = string.Join(" | ", errorMessages);
            }
            return new ApiResult(false, ApiResultStatusCodeEnum.BadRequest, message);
        }

        public static implicit operator ApiResult(ContentResult result)
        {
            return new ApiResult(true, ApiResultStatusCodeEnum.Success, result.Content);
        }

        public static implicit operator ApiResult(NotFoundResult result)
        {
            return new ApiResult(false, ApiResultStatusCodeEnum.NotFound);
        }
        #endregion
    }


    public partial class ApiResult<TData> : ApiResult
        where TData : class
    {
        public ApiResult(bool isSuccess, ApiResultStatusCodeEnum customStatusCode, TData? data, string? message = null)
            : base(isSuccess, customStatusCode, message)
        {
            Data = data;
        }

        #region Implicit Operators
        public static implicit operator ApiResult<TData>(TData data)
        {
            return new ApiResult<TData>(true, ApiResultStatusCodeEnum.Success, data);
        }

        public static implicit operator ApiResult<TData>(OkResult result)
        {
            return new ApiResult<TData>(true, ApiResultStatusCodeEnum.Success, null);
        }

        public static implicit operator ApiResult<TData>(OkObjectResult result)
        {
            return new ApiResult<TData>(true, ApiResultStatusCodeEnum.Success, (TData)result.Value);
        }

        public static implicit operator ApiResult<TData>(BadRequestResult result)
        {
            return new ApiResult<TData>(false, ApiResultStatusCodeEnum.BadRequest, null);
        }

        public static implicit operator ApiResult<TData>(BadRequestObjectResult result)
        {
            var message = result.Value.ToString();
            if (!(result.Value is SerializableError errors))
                return new ApiResult<TData>(false, ApiResultStatusCodeEnum.BadRequest, null, message);
            var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
            message = string.Join(" | ", errorMessages);
            return new ApiResult<TData>(false, ApiResultStatusCodeEnum.BadRequest, null, message);
        }

        public static implicit operator ApiResult<TData>(ContentResult result)
        {
            return new ApiResult<TData>(true, ApiResultStatusCodeEnum.Success, null, result.Content);
        }

        public static implicit operator ApiResult<TData>(NotFoundResult result)
        {
            return new ApiResult<TData>(false, ApiResultStatusCodeEnum.NotFound, null);
        }

        public static implicit operator ApiResult<TData>(NotFoundObjectResult result)
        {
            return new ApiResult<TData>(false, ApiResultStatusCodeEnum.NotFound, (TData)result.Value);
        }
        #endregion
    }


}