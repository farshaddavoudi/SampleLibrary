using ATA.Library.Client.Web.Service;
using ATA.Library.Client.Web.Service.AppSetting;
using Microsoft.Extensions.DependencyInjection;

namespace ATA.Library.Client.Web.UI.Infrastructure.AppSingletonCaches
{
    public class AppCacheInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings)
        {
            var appCache = new AppData();

            services.AddSingleton(sp => appCache);
        }
    }
}