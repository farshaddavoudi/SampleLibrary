using ATA.Library.Server.Data;
using ATA.Library.Server.Data.Interceptors;
using ATA.Library.Server.Model.AppSettings;
using ATA.Library.Server.Model.Entities.Contracts.AuditLog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ATA.Library.Server.Api.Infrastructure.EfDbContext
{
    public class DbContextInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings, IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            services.AddDbContext<ATADbContext>((serviceProvider, options) =>
            {
                options
                    .UseSqlServer(appSettings.ConnectionStrings!.AppDbConnectionString!, sqlServerOptionsBuilder =>
                    {
                        sqlServerOptionsBuilder.CommandTimeout((int)TimeSpan.FromMinutes(1)
                            .TotalSeconds); //Default is 30 seconds
                        sqlServerOptionsBuilder.EnableRetryOnFailure();
                    });
                //Tips

                // Interceptors
                var entityAuditProvider = serviceProvider.GetRequiredService<IEntityAuditProvider>();
                options.AddInterceptors(new AuditSaveChangesInterceptor(entityAuditProvider));

                // Show Detailed Errors
                if (hostEnvironment.IsDevelopment())
                    options.EnableSensitiveDataLogging().EnableDetailedErrors();

                // Activate EF Second Level Cache
                if (appSettings.EFSecondLevelCacheSettings!.Enabled) { }
                //options.AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>());
            });


        }
    }
}