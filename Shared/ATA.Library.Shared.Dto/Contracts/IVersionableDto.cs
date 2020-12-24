
namespace ATA.Library.Shared.Dto
{
    public interface IVersionableDto : IDto
    {
        long Version { get; set; }
    }
}
