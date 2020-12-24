using ATA.Core.Authentication;
using ATA.Core.Authorization;
using ATA.Library.Server.Model.AppSettings;
using ATA.Library.Shared.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

namespace ATA.Library.Server.Api.Infrastructure.Authentication
{
    public class ATASSOInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings, IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            var appName = AppStrings.ATASecurityAppKeyName;

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "SSO Scheme";
                    options.DefaultChallengeScheme = "SSO Scheme";
                })
                .AddCustomAuthentication(o => { });

            services.AddSingleton(new HttpClient());

            services.AddMvc(config =>
            {
                config.Filters.Add(SSOAuthorization.GetAuthorizeFilter(appName));
            });
        }
    }
}