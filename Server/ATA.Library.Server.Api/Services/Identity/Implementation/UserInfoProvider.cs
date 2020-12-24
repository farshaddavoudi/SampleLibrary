using ATA.Library.Server.Service.Contracts;
using ATA.Library.Shared.Dto;
using ATA.Library.Shared.Service.Exceptions;
using ATA.Library.Shared.Service.Extensions;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace ATA.Library.Server.Api.Services.Identity.Implementation
{
    public class UserInfoProvider : IUserInfoProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        #region Constructor Injections

        public UserInfoProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        public UserDto? CurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (!user!.Identity!.IsAuthenticated) return null; //Not authorized

            var claims = user.Claims.ToList();

            if (claims == null)
                throw new BadRequestException("اطلاعات توکن معتبر نیست");

            return new UserDto
            {
                SsoToken = claims.Where(x => x.Type == "Token").Select(x => x.Value).First(),
                Id = claims.Where(x => x.Type == "UserID").Select(x => x.Value).First().ToInt(),
                FirstName = claims.Where(x => x.Type == "FName").Select(x => x.Value).First(),
                LastName = claims.Where(x => x.Type == "LName").Select(x => x.Value).First(),
                PersonnelCode = claims.Where(x => x.Type == "InfperCode").Select(x => x.Value).First().ToInt()
            };

        }

        public int UserId()
        {
            return CurrentUser()!.Id;
        }

        public string? IpAddress()
        {
            return null;
        }

    }
}