using ATA.Library.Client.Web.Service.AppSetting;
using ATA.Library.Client.Web.UI.Infrastructure;
using ATA.Library.Client.Web.UI.Infrastructure.SyncfusionComponent;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace ATA.Library.Client.Web.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            SyncfusionLicense.Register();

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var blazorAppSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

            // Client Installers
            builder.Services.InstallServicesInAssemblies(blazorAppSettings);

            await builder
                .Build()
                .UseLoadingBar()
                .RunAsync();

        }
    }
}
