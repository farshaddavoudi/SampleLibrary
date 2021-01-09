using ATA.Library.Client.Web.Service.AppSetting;
using ATA.Library.Shared.Dto;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace ATA.Library.Client.Web.UI.Infrastructure.AppSingletonCaches
{
    public class AppCacheInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings)
        {
            var appCache = new AppCache();

            services.AddSingleton(sp => appCache);
        }
    }

    public class AppCache
    {
        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
    }
}