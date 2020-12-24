using System;

namespace ATA.Library.Server.Model.Entities.Contracts
{
    public interface IATAEntity<TKey> : IArchivableEntity, IAuditableEntity
    {
        TKey Id { get; set; }

        DateTime CreatedAt { get; set; }

        DateTime ModifiedAt { get; set; }

    }

    public interface IATAEntity : IATAEntity<int>
    {

    }


}