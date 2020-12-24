using ATA.Library.Client.Web.Service.AppSetting;
using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;

namespace ATA.Library.Client.Web.UI.Infrastructure.BrowserStorage
{
    public class LocalStorageInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings)
        {
            // https://github.com/Blazored/LocalStorage
            services.AddBlazoredLocalStorage();
        }
    }
}