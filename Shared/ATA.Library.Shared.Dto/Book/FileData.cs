using ATA.Library.Server.Model.Enums;

namespace ATA.Library.Server.Model.Book
{
    public class FileData
    {
        public byte[]? Data { get; set; }

        public string? MimeType { get; set; }

        public FileType FileType { get; set; }

        public long Size { get; set; }
    }
}