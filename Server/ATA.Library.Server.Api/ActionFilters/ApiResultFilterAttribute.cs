using ATA.Library.Shared.Dto;
using ATA.Library.Shared.Service.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Linq;

namespace ATA.Library.Server.Api.ActionFilters
{
    public class ApiResultFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult okObjectResult)
            {
                var instanceTypeName = okObjectResult.Value.GetType().Name;
                if (instanceTypeName != nameof(ApiResult))
                {
                    var apiResult = new ApiResult<object>(true, ApiResultStatusCodeEnum.Success, okObjectResult.Value);
                    context.Result = new JsonResult(apiResult) { StatusCode = okObjectResult.StatusCode };
                }
            }

            else if (context.Result is ObjectResult createObjectResult && createObjectResult.StatusCode == 201)
            {
                var apiResult = new ApiResult<object>(true, ApiResultStatusCodeEnum.Success, createObjectResult.Value);
                context.Result = new JsonResult(apiResult) { StatusCode = createObjectResult.StatusCode };
            }
            else if (context.Result is OkResult okResult)
            {
                var apiResult = new ApiResult(true, ApiResultStatusCodeEnum.Success);
                context.Result = new JsonResult(apiResult) { StatusCode = okResult.StatusCode };
            }

            //return BadRequest() method create an ObjectResult with StatusCode 400 in recent versions, So the following code has changed a bit.
            else if (context.Result is ObjectResult badRequestObjectResult && badRequestObjectResult.StatusCode == 400)
            {
                string? message = null;
                switch (badRequestObjectResult.Value)
                {
                    case ValidationProblemDetails validationProblemDetails:
                        var errorMessages = validationProblemDetails.Errors.SelectMany(p => p.Value).Distinct();
                        message = string.Join(" | ", errorMessages);
                        break;
                    case SerializableError errors:
                        var errorMessages2 = errors.SelectMany(p => (string[])p.Value).Distinct();
                        message = string.Join(" | ", errorMessages2);
                        break;
                    case var value when value != null && !(value is ProblemDetails):
                        message = badRequestObjectResult.Value.ToString();
                        break;
                }

                var apiResult = new ApiResult(false, ApiResultStatusCodeEnum.BadRequest, message);
                context.Result = new JsonResult(apiResult) { StatusCode = badRequestObjectResult.StatusCode };
            }

            else if (context.Result is ObjectResult notFoundObjectResult && notFoundObjectResult.StatusCode == 404)
            {
                string? message = null;
                if (notFoundObjectResult.Value != null && !(notFoundObjectResult.Value is ProblemDetails))
                    message = notFoundObjectResult.Value.ToString();

                //var apiResult = new ApiResult<object>(false, ApiResultStatusCodeEnum.NotFound, notFoundObjectResult.Value);
                var apiResult = new ApiResult(false, ApiResultStatusCodeEnum.NotFound, message);
                context.Result = new JsonResult(apiResult) { StatusCode = notFoundObjectResult.StatusCode };
            }

            else if (context.Result is ContentResult contentResult)
            {
                var content = contentResult.ContentType == "application/json"
                    ? JsonConvert.DeserializeObject(contentResult.Content) : contentResult.Content;

                var apiResult = new ApiResult<object>(true, ApiResultStatusCodeEnum.Success, content!);
                context.Result = new JsonResult(apiResult) { StatusCode = contentResult.StatusCode };
            }

            else if (context.Result is ObjectResult objectResult && objectResult.StatusCode == null
                && !(objectResult.Value is ApiResult))
            {
                var apiResult = new ApiResult<object>(true, ApiResultStatusCodeEnum.Success, objectResult.Value);
                context.Result = new JsonResult(apiResult) { StatusCode = objectResult.StatusCode };
            }

            base.OnResultExecuting(context);
        }
    }
}
