using ATA.Library.Client.Web.Service.AppSetting;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace ATA.Library.Client.Web.UI.Infrastructure.HttpLoadingBar
{
    public class LoadingBarInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings)
        {
            services.AddLoadingBar();
        }
    }
}