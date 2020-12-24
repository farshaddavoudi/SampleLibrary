using ATA.Library.Server.Model.Entities.AuditLogAssets;
using ATA.Library.Server.Model.Entities.Contracts;
using ATA.Library.Server.Model.Entities.Contracts.AuditLog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ATA.Library.Server.Data.Interceptors
{
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IEntityAuditProvider _entityAuditProvider;

        #region Constructor Injections
        public AuditSaveChangesInterceptor(IEntityAuditProvider entityAuditProvider)
        {
            _entityAuditProvider = entityAuditProvider;
        }

        #endregion

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            ApplyAudits(eventData.Context.ChangeTracker);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            ApplyAudits(eventData.Context.ChangeTracker);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ApplyAudits(ChangeTracker changeTracker)
        {
            ApplyCreateAudits(changeTracker);
            ApplyUpdateAudits(changeTracker);
            ApplyDeleteAudits(changeTracker);
        }

        private void ApplyCreateAudits(ChangeTracker changeTracker)
        {
            var addedEntries = changeTracker.Entries()
                .Where(x => x.State == EntityState.Added);

            foreach (var addedEntry in addedEntries)
            {
                if (addedEntry.Entity is IATAEntity entity)
                {
                    entity.CreatedAt = DateTime.Now;
                    entity.ModifiedAt = DateTime.Now;
                    entity.Audit = _entityAuditProvider.GetAuditValues(EntityEventType.Create, entity);
                }
            }
        }

        private void ApplyUpdateAudits(ChangeTracker changeTracker)
        {
            var modifiedEntries = changeTracker.Entries()
                .Where(x => x.State == EntityState.Modified);

            foreach (var modifiedEntry in modifiedEntries)
            {
                if (modifiedEntry.Entity is IATAEntity entity)
                {
                    entity.ModifiedAt = DateTime.Now;
                    var eventType = entity.IsArchived ? EntityEventType.Delete : EntityEventType.Update; // Maybe Soft Delete
                    entity.Audit = _entityAuditProvider.GetAuditValues(eventType, entity, entity.Audit);
                }
            }
        }

        private void ApplyDeleteAudits(ChangeTracker changeTracker)
        {
            var deletedEntries = changeTracker.Entries()
                .Where(x => x.State == EntityState.Deleted);

            foreach (var modifiedEntry in deletedEntries)
            {
                if (modifiedEntry.Entity is IATAEntity entity)
                {
                    entity.ModifiedAt = DateTime.Now;
                    entity.Audit = _entityAuditProvider.GetAuditValues(EntityEventType.Delete, entity, entity.Audit);
                }
            }
        }

    }
}