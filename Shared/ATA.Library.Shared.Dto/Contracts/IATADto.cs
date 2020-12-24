using System;

namespace ATA.Library.Client.Dto.Contracts
{
    public interface IATADto : IArchivableDto
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

    }
}
