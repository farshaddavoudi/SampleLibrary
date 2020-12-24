using ATA.Library.Client.Web.Service.AppSetting;
using Microsoft.Extensions.DependencyInjection;

namespace ATA.Library.Client.Web.UI.Infrastructure.AppSetting
{
    public class AppSettingsInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings)
        {
            // Directly with injecting AppSettings class
            services.AddSingleton(serviceProvider => appSettings);
        }
    }
}