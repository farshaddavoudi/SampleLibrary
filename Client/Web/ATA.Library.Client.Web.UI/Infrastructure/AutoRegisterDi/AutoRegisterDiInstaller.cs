using ATA.Library.Client.Service.SecurityClient;
using ATA.Library.Client.Web.Service.AppSetting;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;
using System.Reflection;

namespace ATA.Library.Client.Web.UI.Infrastructure.AutoRegisterDi
{
    public class AutoRegisterDiInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings)
        {
            var uiAssembly = Assembly.GetExecutingAssembly();
            var clientServiceAssembly = typeof(SecurityClientService).Assembly;
            var clientWebServiceAssembly = typeof(AppSettings).Assembly;

            var assembliesToScan = new[] { uiAssembly, clientWebServiceAssembly, clientServiceAssembly };

            #region Generic Type Dependencies
            //services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepositoryBase<,>));
            #endregion


            #region Register DIs By Name
            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
                .Where(c => c.Name.EndsWith("Service"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);
            #endregion 
        }
    }
}