using System.ServiceProcess;
using ATA.Library.Server.Api.Infrastructure.HealthCheck.HealthChecks;
using ATA.Library.Server.Model.AppSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ATA.Library.Server.Api.Infrastructure.HealthCheck
{
    public class HealthCheckInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings, IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            services.AddHealthChecks()
                .AddSqlServer(appSettings.ConnectionStrings!.AppDbConnectionString,
                    name: "SQLServer",
                    tags: new[] {"database"})

                .AddDiskStorageHealthCheck(s => s.AddDrive("C:\\", 10240), // 10240 MB (10 GB) free minimum
                    name: "DiskStorage",
                    tags: new[] {"server"})

                .AddCheck<ATASecurityHealthCheck>("ATASecurity", null, new[] {"service"});
        }
    }
}