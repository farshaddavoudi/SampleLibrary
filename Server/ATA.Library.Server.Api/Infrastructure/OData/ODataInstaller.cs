using ATA.Library.Server.Model.AppSettings;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;

namespace ATA.Library.Server.Api.Infrastructure.OData
{
    public class ODataInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings, IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            services.AddOData();

            services.AddMvcCore(options =>
            {
                IEnumerable<ODataInputFormatter> inputFormatters =
                    options.InputFormatters.OfType<ODataInputFormatter>()
                        .Where(formatter => formatter.SupportedMediaTypes.Count == 0);

                IEnumerable<ODataOutputFormatter> outputFormatters =
                    options.OutputFormatters.OfType<ODataOutputFormatter>()
                        .Where(formatter => formatter.SupportedMediaTypes.Count == 0);

                foreach (var inputFormatter in inputFormatters)
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/odata"));
                }

                foreach (var outputFormatter in outputFormatters)
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/odata"));
                }
            });
        }
    }
}