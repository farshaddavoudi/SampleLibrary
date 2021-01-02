using ATA.Library.Server.Api.Infrastructure;
using ATA.Library.Server.Api.Infrastructure.EfDbContext;
using ATA.Library.Server.Api.Infrastructure.OData;
using ATA.Library.Server.Api.Infrastructure.Swagger;
using ATA.Library.Server.Api.Middlewares;
using ATA.Library.Server.Model.AppSettings;
using HealthChecks.UI.Client;
using Humanizer;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ATA.Library.Server.Api
{
    public class Startup
    {
        #region Constructor Injections

        private readonly AppSettings _appSettings;
        public IConfiguration Configuration { get; }

        private readonly IHostEnvironment _hostEnvironment;
        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            _hostEnvironment = hostEnvironment;
            _appSettings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
        }

        #endregion


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //* HttpContextAccessor
            services.AddHttpContextAccessor();

            services.AddControllers(options =>
            {
                //options.Filters.Add(new AuthorizeFilter());
                options.Conventions.Add(new RouteTokenTransformerConvention(
                    new CamelCaseDasherizeParameterTransformer()));
            })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            //* Installers
            services.InstallServicesInAssemblies(_appSettings, Configuration, _hostEnvironment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ExecuteMigrations(env);

            app.UseRequestResponseLogging();

            app.UseCustomExceptionHandler();

            if (env.IsProduction())
                app.UseHsts();


            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            // global Cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            app.UseAuthentication();

            app.UseAuthorization();

            // app.UseCustomHangfire(_appSettings.JobSettings!);

            app.UseSwaggerAndUI(env);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapControllers();
                endpoints.EnableDependencyInjection();
                endpoints.Select().Filter().Expand().MaxTop(100).OrderBy().Count();
                endpoints.MapODataRoute("odata", "odata", ODataEdmModelsConfiguration.GetEdmModels());
            });
        }

        private class CamelCaseDasherizeParameterTransformer : IOutboundParameterTransformer
        {
            public string? TransformOutbound(object? value)
            {
                return value?.ToString().Kebaberize();
            }
        }

    }
}
