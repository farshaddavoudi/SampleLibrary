using ATA.Library.Server.Model.AppSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;

namespace ATA.Library.Server.Api.Infrastructure
{
    public static class ServiceInstallerExtensions
    {
        public static void InstallServicesInAssemblies(this IServiceCollection services, AppSettings appSettings,
            IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            var assemblies = new[] { Assembly.GetExecutingAssembly() };

            var installers = assemblies.SelectMany(a => a.GetExportedTypes())
                .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(IServiceInstaller).IsAssignableFrom(c))
                .Select(Activator.CreateInstance).Cast<IServiceInstaller>().ToList();

            installers.ForEach(i => i.InstallServices(services, appSettings, configuration, hostEnvironment));
        }
    }
}