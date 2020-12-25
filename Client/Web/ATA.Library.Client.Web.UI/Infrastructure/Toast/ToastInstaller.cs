using ATA.Library.Client.Web.Service.AppSetting;
using Blazored.Toast;
using Microsoft.Extensions.DependencyInjection;

namespace ATA.Library.Client.Web.UI.Infrastructure.Toast
{
    public class ToastInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings)
        {
            services.AddBlazoredToast();
        }
    }
}