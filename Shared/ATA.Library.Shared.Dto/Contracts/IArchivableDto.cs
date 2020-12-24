namespace ATA.Library.Client.Dto.Contracts
{
    public interface IArchivableDto : IDto
    {
        bool IsArchived { get; set; }
    }
}
