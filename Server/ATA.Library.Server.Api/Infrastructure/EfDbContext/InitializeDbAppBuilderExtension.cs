using ATA.Library.Server.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ATA.Library.Server.Api.Infrastructure.EfDbContext
{
    public static class InitializeDbAppBuilderExtension
    {
        public static void ExecuteMigrations(this IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsProduction())
            {
                using var serviceScope = app.ApplicationServices
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope();
                using var context = serviceScope.ServiceProvider.GetService<ATADbContext>();
                context?.Database.Migrate();
            }
        }
    }
}