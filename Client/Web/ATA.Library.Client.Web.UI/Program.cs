using ATA.Library.Client.Web.Service.AppSetting;
using ATA.Library.Client.Web.UI.Infrastructure;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var blazorAppSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

            // Client Installers
            builder.Services.InstallServicesInAssemblies(blazorAppSettings);

            await builder.Build().RunAsync();
        }
    }
}
