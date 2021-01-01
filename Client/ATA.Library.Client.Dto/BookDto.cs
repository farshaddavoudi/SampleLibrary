using Humanizer;

// ReSharper disable CheckNamespace
namespace ATA.Library.Shared.Dto
{
    public partial class BookDto
    {
        public string? BookSizeToDisplay => BookFileSize.Bytes().Humanize("0.0");
    }
}