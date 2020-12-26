using ATA.Library.Client.Dto.HttpTypedClients.ATASecurityClient;
using ATA.Library.Client.Dto.HttpTypedClients.ATASecurityClient.Dtos;
using ATA.Library.Client.Service.SecurityClient.Contract;
using ATA.Library.Shared.Core;
using ATA.Library.Shared.Dto;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ATA.Library.Client.Service.SecurityClient
{
    public class SecurityClientService : ISecurityClientService
    {
        private readonly HttpClient _securityClient;

        #region Constructor Injections

        public SecurityClientService(ATASecurityClient securityClientFactory)
        {
            _securityClient = securityClientFactory.Client;
        }

        #endregion

        public async Task<SecurityResult<UserByTokenData?>?> GetUserByTokenAsync(string token,
            CancellationToken cancellationToken)
        {
            return await _securityClient.GetFromJsonAsync<SecurityResult<UserByTokenData?>>(
                $"api/Authentication/GetByToken?token={token}", cancellationToken);
        }

        public async Task<SecurityResult<UserByTokenData?>?> GetUserTokenByPersonnelCodeAsync(int personnelCode, CancellationToken cancellationToken)
        {
            return await _securityClient.GetFromJsonAsync<SecurityResult<UserByTokenData?>>(
                $"api/Authentication/DirectLogin?perscode={personnelCode}&version=1&remarks=farshad", cancellationToken);
        }

        public async Task<List<UserRoleResponseDto>?> GetUserRolesAsync(string token, CancellationToken cancellationToken)
        {
            return await GetFromClientAsync<List<UserRoleResponseDto>>(
                $"api/Authentication/GetUserRolesInApp?token={token}&ressource=abc&app={AppStrings.ATASecurityAppKeyName}",
                nameof(GetUserRolesAsync), cancellationToken);
        }

        private async Task<T?> GetFromClientAsync<T>(string requestUri, string? methodName = null, CancellationToken cancellationToken = default)
            where T : class
        {
            var response = await _securityClient.GetFromJsonAsync<SecurityResult<T?>>(requestUri, cancellationToken);

            if (response == null || !response.IsSuccessful)
            {
                var fullMethodNameMessage = methodName == null ? string.Empty : $"in method: {methodName}";
                throw new HttpRequestException($"Failed get call to Security client {fullMethodNameMessage}");
            }

            return response.Data;
        }
    }
}