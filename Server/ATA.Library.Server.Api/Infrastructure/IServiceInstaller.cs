using ATA.Library.Server.Model.AppSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ATA.Library.Server.Api.Infrastructure
{
    public interface IServiceInstaller
    {
        void InstallServices(IServiceCollection services,
            AppSettings appSettings,
            IConfiguration configuration,
            IHostEnvironment hostEnvironment);
    }
}