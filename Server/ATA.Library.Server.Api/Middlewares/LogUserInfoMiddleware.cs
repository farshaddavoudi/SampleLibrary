using ATA.Library.Server.Service.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Threading.Tasks;

namespace ATA.Library.Server.Api.Middlewares
{
    public static class LogUserInfoMiddlewareExtensions
    {
        public static IApplicationBuilder LogUserInfoIntoSerilog(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogUserInfoMiddleware>();
        }

        public class LogUserInfoMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly IUserInfoProvider _userInfoProvider;

            public LogUserInfoMiddleware(RequestDelegate next, IUserInfoProvider userInfoProvider)
            {
                _next = next;
                _userInfoProvider = userInfoProvider;
            }

            public Task Invoke(HttpContext context)
            {
                var userId = _userInfoProvider.CurrentUser()?.Id;

                var firstName = _userInfoProvider.CurrentUser()?.FirstName;

                var lastName = _userInfoProvider.CurrentUser()?.LastName;

                var personnelCode = _userInfoProvider.CurrentUser()?.PersonnelCode;

                var info = userId == null ? "Guest" : $"{firstName} {lastName} | UserId: {userId} | PersonnelCode: {personnelCode}";

                LogContext.PushProperty("User", info ?? "Guest");

                return _next(context);
            }
        }
    }


}