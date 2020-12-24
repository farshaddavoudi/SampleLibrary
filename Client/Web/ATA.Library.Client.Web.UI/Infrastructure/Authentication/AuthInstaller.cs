using ATA.Library.Client.Web.Service.AppSetting;
using ATA.Library.Client.Web.UI.AuthProvider;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace ATA.Library.Client.Web.UI.Infrastructure.Authentication
{
    public class AuthInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings)
        {
            services.AddOptions();

            services.AddAuthorizationCore();

            services.AddScoped<AuthenticationStateProvider, AppAuthenticationStateProvider>();
        }
    }
}