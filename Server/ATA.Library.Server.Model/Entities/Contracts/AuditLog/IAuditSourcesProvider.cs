using ATA.Library.Server.Model.Entities.AuditLogAssets;

namespace ATA.Library.Server.Model.Entities.Contracts.AuditLog
{
    public interface IAuditSourcesProvider
    {
        AuditSourceValues GetAuditSourceValues();
    }
}