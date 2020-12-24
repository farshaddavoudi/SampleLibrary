using ATA.Library.Server.Model.Entities.Contracts;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ATA.Library.Server.Model.Entities
{
    public abstract class ATAEntity : IATAEntity
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public bool IsArchived { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public DateTime ModifiedAt { get; set; }

        [JsonIgnore]
        public string? Audit { get; set; }
    }
}