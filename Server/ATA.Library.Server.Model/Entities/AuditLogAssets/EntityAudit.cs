using ATA.Library.Shared.Service.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace ATA.Library.Server.Model.Entities.AuditLogAssets
{
    public class EntityAudit<TEntity>
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EntityEventType EventType { get; set; }

        [JsonProperty("user", NullValueHandling = NullValueHandling.Include)]
        public int? ActorUserId { get; set; }

        [JsonProperty("at")]
        public DateTime ActDateTime { get; set; }

        [JsonProperty("sources")]
        public AuditSourceValues? AuditSourceValues { get; set; }

        [JsonProperty("newValues", NullValueHandling = NullValueHandling.Include)]
        public TEntity NewEntity { get; set; } = default!;

        public string? SerializeJson()
        {
            return this.SerializeToJson(true);
        }

        public static AuditSourceValues? DeserializeJson(string? value)
        {
            return value?.DeserializeToModel<AuditSourceValues>();
        }
    }
}