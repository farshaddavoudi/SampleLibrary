using ATA.Library.Client.Web.Service.AppSetting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace ATA.Library.Client.Web.UI.Infrastructure
{
    public static class ServiceInstallerExtensions
    {
        public static void InstallServicesInAssemblies(this IServiceCollection services, AppSettings appSettings)
        {
            var assemblies = new[] { Assembly.GetExecutingAssembly() };

            var installers = assemblies.SelectMany(a => a.GetExportedTypes())
                .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(IServiceInstaller).IsAssignableFrom(c))
                .Select(Activator.CreateInstance).Cast<IServiceInstaller>().ToList();

            installers.ForEach(i => i.InstallServices(services, appSettings));
        }

    }
}