namespace ATA.Library.Shared.Dto
{
    public interface IArchivableDto : IDto
    {
        bool IsArchived { get; set; }
    }
}
