using ATA.Library.Server.Model.AppSettings;
using ATA.Library.Server.Model.HttpTypedClients.ATASecurityClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ATA.Library.Server.Api.Infrastructure.HttpClientFactory
{
    public class HttpClientFactoryInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings, IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            // ATA Security (Typed) HttpClient
            services.AddHttpClient<ATASecurityClient>(client =>
            {
                client.BaseAddress = new Uri(appSettings.Urls!.SecurityBaseUrl!);
            });
        }
    }
}