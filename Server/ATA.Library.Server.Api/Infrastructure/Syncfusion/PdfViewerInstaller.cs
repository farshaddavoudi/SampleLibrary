using ATA.Library.Server.Model.AppSettings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ATA.Library.Server.Api.Infrastructure.Syncfusion
{
    public class PdfViewerInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings, IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression();
        }
    }
}