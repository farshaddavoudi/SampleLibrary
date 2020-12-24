using ATA.Library.Server.Model.AppSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ATA.Library.Server.Api.Infrastructure.AppSetting
{
    public class AppSettingsInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings,
            IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            // Directly with injecting AppSettings class
            services.AddSingleton(serviceProvider => appSettings);

            // With injecting IOptions<AppSettings> 
            services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
        }
    }
}