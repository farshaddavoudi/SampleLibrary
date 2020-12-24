
namespace ATA.Library.Client.Dto.Contracts
{
    public interface IVersionableDto : IDto
    {
        long Version { get; set; }
    }
}
