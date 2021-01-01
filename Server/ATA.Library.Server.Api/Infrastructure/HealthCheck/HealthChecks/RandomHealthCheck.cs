using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ATA.Library.Server.Api.Infrastructure.HealthCheck.HealthChecks
{
    public class RandomHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            await Task.Delay(1);
            return HealthCheckResult.Healthy("It is for test");
        }
    }
}