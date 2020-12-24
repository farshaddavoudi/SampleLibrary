using ATA.Library.Shared.Dto;

namespace ATA.Library.Server.Service.Contracts
{
    public interface IUserInfoProvider
    {
        UserDto? CurrentUser();

        int UserId();

        public string? IpAddress();
    }
}
