namespace ATA.Library.Server.Model.Entities.Contracts
{
    public interface IVersionableEntity
    {
        long Version { get; set; }
    }
}