using ATA.Library.Client.Web.Service.AppSetting;
using Blazored.Modal;
using Microsoft.Extensions.DependencyInjection;

namespace ATA.Library.Client.Web.UI.Infrastructure.Modal
{
    public class ModalInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings)
        {
            // https://github.com/Blazored/Modal
            services.AddBlazoredModal();
        }
    }
}