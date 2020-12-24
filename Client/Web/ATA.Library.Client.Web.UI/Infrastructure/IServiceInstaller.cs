using ATA.Library.Client.Web.Service.AppSetting;
using Microsoft.Extensions.DependencyInjection;

namespace ATA.Library.Client.Web.UI.Infrastructure
{
    public interface IServiceInstaller
    {
        void InstallServices(IServiceCollection services, AppSettings appSettings);
    }
}