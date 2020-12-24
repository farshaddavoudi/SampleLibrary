using ATA.Library.Client.Dto.HttpTypedClients.ATASecurityClient.Dtos;
using ATA.Library.Shared.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ATA.Library.Client.Service.SecurityClient.Contract
{
    public interface ISecurityClientService
    {
        Task<SecurityResult<UserByTokenData?>?> GetUserByTokenAsync(string token,
            CancellationToken cancellationToken);

        Task<SecurityResult<UserByTokenData?>?> GetUserTokenByPersonnelCodeAsync(int personnelCode,
            CancellationToken cancellationToken);

        Task<List<UserRoleResponseDto>?> GetUserRolesAsync(string token, CancellationToken cancellationToken);
    }
}