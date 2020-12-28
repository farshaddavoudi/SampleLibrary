using ATA.Library.Client.Web.Service.AppSetting;
using Microsoft.Extensions.DependencyInjection;
using Syncfusion.Blazor;

namespace ATA.Library.Client.Web.UI.Infrastructure.SyncfusionComponent
{
    public class SyncfusionInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings)
        {
            services.AddSyncfusionBlazor();
        }
    }
}