using ATA.Library.Server.Model.Entities.AuditLogAssets;

namespace ATA.Library.Server.Model.Entities.Contracts.AuditLog
{
    public interface IEntityAuditProvider
    {
        string? GetAuditValues(EntityEventType eventType, object? entity, string? previousJsonAudit = null);
    }
}