using ATA.Library.Server.Api.Controllers;
using ATA.Library.Server.Model.AppSettings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ATA.Library.Server.Api.Infrastructure.Swagger
{
    public class ApiVersioningInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings, IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            services.AddApiVersioning(options =>
            {
                //url segment => {version}
                options.AssumeDefaultVersionWhenUnspecified = false; //default => false;
                options.DefaultApiVersion = new ApiVersion(1, 0); //v1.0 == v1

                options.Conventions.Add(new DefaultODataApiVersion());

                // options.ReportApiVersions = true;

                //options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
                // api/posts?api-version=1

                // options.ApiVersionReader = new UrlSegmentApiVersionReader();
                // api/v1/posts

                //options.ApiVersionReader = new HeaderApiVersionReader(new[] { "Api-Version" });
                // header => Api-Version : 1

                //options.ApiVersionReader = new MediaTypeApiVersionReader()

                //options.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("api-version"), new UrlSegmentApiVersionReader())
                // combine of [querystring] & [urlsegment]
            });
        }
    }

    public class DefaultODataApiVersion : IControllerConvention
    {
        public bool Apply(IControllerConventionBuilder controller, ControllerModel controllerModel)
        {
            if (controller.ControllerType.BaseType == typeof(BaseODataController))
            {
                controller.HasApiVersion(1, 0);
                return true;
            }

            return false;
        }
    }
}