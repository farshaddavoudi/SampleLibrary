using ATA.Library.Server.Model.AppSettings;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Seq.Api;
using System.Threading;
using System.Threading.Tasks;

namespace ATA.Library.Server.Api.Infrastructure.HealthCheck.HealthChecks
{
    public class SeqHealthCheck : IHealthCheck
    {
        private readonly AppSettings _appSettings;

        #region Constructor Injections

        public SeqHealthCheck(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        #endregion

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var connection = new SeqConnection(_appSettings.Seq!.ServerUrl, _appSettings.Seq!.ApiKey);

            // If Seq is not running, below code will fail and HealthCheck result would be unhealthy.
            await connection.Apps.ListAsync(cancellationToken);

            return HealthCheckResult.Healthy("Logger Service is OK");
        }
    }
}