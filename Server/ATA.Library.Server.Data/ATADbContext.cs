using ATA.Library.Server.Data.ContextBase;
using ATA.Library.Server.Data.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ATA.Library.Server.Data
{
    public class ATADbContext : EfCoreDbContextBase
    {
        public ATADbContext(DbContextOptions<ATADbContext> options,
            IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Auto Register all Entities
            modelBuilder.RegisterDbSets();

            modelBuilder.UseJsonDbFunctions();

            base.OnModelCreating(modelBuilder);

            modelBuilder.RegisterIsArchivedGlobalQueryFilter();

            modelBuilder.ConfigureDecimalPrecision();

            // Restrict Delete (in Hard delete scenarios)
            // Ef default is Cascade
            modelBuilder.SetRestrictAsDefaultDeleteBehavior();

            // Auto Register all Entity Configurations (Fluent-API)
            modelBuilder.ApplyConfigurations(typeof(ATADbContext).Assembly);
        }
    }
}