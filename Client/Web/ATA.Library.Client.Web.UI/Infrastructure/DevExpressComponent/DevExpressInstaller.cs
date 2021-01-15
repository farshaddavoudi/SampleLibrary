using ATA.Library.Client.Web.Service.AppSetting;
using Microsoft.Extensions.DependencyInjection;

namespace ATA.Library.Client.Web.UI.Infrastructure.DevExpressComponent
{
    public class DevExpressInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings)
        {
            services.AddDevExpressBlazor();
        }
    }
}