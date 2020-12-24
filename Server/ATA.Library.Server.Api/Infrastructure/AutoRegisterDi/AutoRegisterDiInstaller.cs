using ATA.Library.Server.Data;
using ATA.Library.Server.Data.RepositoryBase;
using ATA.Library.Server.Model.AppSettings;
using ATA.Library.Server.Service;
using ATA.Library.Server.Service.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCore.AutoRegisterDi;
using System.Reflection;

namespace ATA.Library.Server.Api.Infrastructure.AutoRegisterDi
{
    public class AutoRegisterDiInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings, IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {

            var dataAssembly = typeof(ATADbContext).Assembly;
            var serviceAssembly = typeof(EntityService<>).Assembly;
            var apiAssembly = Assembly.GetExecutingAssembly();
            var assembliesToScan = new[] { dataAssembly, serviceAssembly, apiAssembly };

            #region Generic Type Dependencies
            services.AddScoped(typeof(IATARepository<>), typeof(ATARepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepositoryBase<,>));
            //services.AddScoped(typeof(ATA.Server.Service.Contract.IGlobalCachingProvider), typeof(ATA.Server.Service.GlobalCachingProvider));
            #endregion


            #region Register DIs By Name
            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
                .Where(c => c.Name.EndsWith("Repository"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
                .Where(c => c.Name.EndsWith("Service"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
              .Where(c => c.Name.EndsWith("Provider"))
              .AsPublicImplementedInterfaces();

            #endregion 
        }
    }
}