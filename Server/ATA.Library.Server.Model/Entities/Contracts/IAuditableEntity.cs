namespace ATA.Library.Server.Model.Entities.Contracts
{
    public interface IAuditableEntity : IEntity
    {
        string? Audit { get; set; }
    }
}