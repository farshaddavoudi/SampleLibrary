using ATA.Library.Server.Data;
using ATA.Library.Server.Model.AppSettings;
using ATA.Library.Server.Service;
using ATA.Library.Shared.Core;
using ATA.Library.Shared.Service.Extensions;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace ATA.Library.Server.Api.Infrastructure.AutoMapper
{
    public class AutoMapperInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings, IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            //With AutoMapper Instance, you need to call AddAutoMapper services and pass assemblies that contains AutoMapper Profile class
            //services.AddAutoMapper(assembly1, assembly2, assembly3);
            //See http://docs.automapper.org/en/stable/Configuration.html
            //And https://code-maze.com/automapper-net-core/

            services.AddAutoMapper(config =>
            {
                config.Advanced.BeforeSeal(configProvider =>
                {
                    configProvider.CompileMappings();
                });
            }, GetMappingRelatedAssembliesToScan());
        }

        private Assembly[] GetMappingRelatedAssembliesToScan()
        { // Targets are Profile classes and IHaveCustomMapping interfaces
            return new[]
            {
                typeof(AppSettings).Assembly, // Model Assembly
                typeof(ATADbContext).Assembly, // Data Assembly
                typeof(EntityService<>).Assembly, // Service Assembly
                typeof(Startup).Assembly, // Api Assembly
                typeof(AppStrings).Assembly, // Shared.Core Assembly
                typeof(EnumExtensions).Assembly // Shared.Service Assembly
            };
        }


    }
}
