
namespace ATA.Library.Server.Model.Entities.Contracts
{
    public interface IArchivableEntity : IEntity
    {
        bool IsArchived { get; set; }
    }
}