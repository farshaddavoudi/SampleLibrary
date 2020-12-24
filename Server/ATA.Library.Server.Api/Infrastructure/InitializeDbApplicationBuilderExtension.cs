using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace ATA.Library.Server.Api.Infrastructure
{
    public static class InitializeDbApplicationBuilderExtension
    {
        public static void InitializeDatabase(this IApplicationBuilder app, IHostEnvironment env)
        {
            //if (env.IsProduction())
            //{
            //    using var serviceScope = app.ApplicationServices
            //        .GetRequiredService<IServiceScopeFactory>()
            //        .CreateScope();
            //    using var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
            //    context.Database.Migrate();
            //}
        }
    }
}