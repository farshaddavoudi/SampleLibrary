using ATA.Library.Shared.Service.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ATA.Library.Server.Data.ContextBase
{
    public interface ISyncDbContext
    {
        bool IsSyncDbContext { get; set; }
    }

    public abstract class EfCoreDbContextBase : DbContext, ISyncDbContext
    {
        bool ISyncDbContext.IsSyncDbContext { get; set; }

        #region Constructor Injections
        protected IHttpContextAccessor HttpContextAccessor;

        protected EfCoreDbContextBase(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        protected EfCoreDbContextBase(DbContextOptions options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        #endregion

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnSaveChanges();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnSaveChanges();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected virtual void OnSaveChanges()
        {
            CleanRelatedPersianChars();
        }

        // Convert Arabic to Persian chars & Persian digits to English digits
        private void CleanRelatedPersianChars()
        {
            var changedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var item in changedEntities)
            {
                if (item.Entity == null)
                    continue;

                var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

                foreach (var property in properties)
                {
                    var propName = property.Name;

                    var val = property.GetValue(item.Entity, null)?.ToString();

                    if (string.IsNullOrWhiteSpace(val)) continue;
                    var newVal = val.Fa2EnDigits().FixPersianChars();
                    if (newVal == val)
                        continue;
                    property.SetValue(item.Entity, newVal, null);
                }
            }
        }
    }
}