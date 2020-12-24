using System;
using ATA.Library.Client.Dto.HttpTypedClients.ATASecurityClient;
using ATA.Library.Client.Web.Service.AppSetting;
using Microsoft.Extensions.DependencyInjection;

namespace ATA.Library.Client.Web.UI.Infrastructure.HttpClient
{
    public class HttpClientFactoryInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings)
        {
            services.AddHttpClient<ATASecurityClient>(client =>
            {
                client.BaseAddress = new Uri(appSettings.Urls!.SecurityAppUrl!);
            });
        }
    }
}