using ATA.Library.Client.Web.Service.AppSetting;
using Microsoft.Extensions.DependencyInjection;
using System;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace ATA.Library.Client.Web.UI.Infrastructure.HttpClient
{
    public class HostClientInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings)
        {
            // It's Blazor base client. It's lifetime is Singleton, so any auth header can be added later to it.
            // Any HttpClient injected to a class or component will have these configs.
            services.AddSingleton(serviceProvider => new System.Net.Http.HttpClient
            {
                BaseAddress = new Uri(appSettings.Urls!.HostUrl!)
            }.EnableIntercept(serviceProvider));
        }
    }
}