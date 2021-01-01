using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using ATA.Library.Server.Model.HttpTypedClients.ATASecurityClient;
using ATA.Library.Shared.Dto;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ATA.Library.Server.Api.Infrastructure.HealthCheck.HealthChecks
{
    public class ATASecurityHealthCheck : IHealthCheck
    {
        private readonly HttpClient _securityClient;

        #region Constructor Injections

        public ATASecurityHealthCheck(ATASecurityClient securityClientFactory)
        {
            _securityClient = securityClientFactory.Client;
        }

        #endregion

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var securityResponse = await _securityClient.GetFromJsonAsync<SecurityResult<dynamic?>>(
                $"api/Authentication/DirectLogin?perscode=980923&version=1&remarks=farshad", cancellationToken);

            if (securityResponse == null)
                return HealthCheckResult.Unhealthy("There is no result from security.app.ataair.ir");

            if (securityResponse.IsSuccessful)
                return HealthCheckResult.Healthy("SSO Service is OK");

            return HealthCheckResult.Unhealthy(securityResponse.Message);
        }
    }
}