using ATA.Library.Shared.Service.Extensions;
using Newtonsoft.Json;

namespace ATA.Library.Server.Model.Entities.AuditLogAssets
{
    public class AuditSourceValues
    {
        [JsonProperty("hn")]
        public string? HostName { get; set; }

        [JsonProperty("mn")]
        public string? MachineName { get; set; }

        [JsonProperty("rip")]
        public string? RemoteIpAddress { get; set; }

        [JsonProperty("lip")]
        public string? LocalIpAddress { get; set; }

        [JsonProperty("ua")]
        public string? UserAgent { get; set; }

        [JsonProperty("an")]
        public string? ApplicationName { get; set; }

        [JsonProperty("av")]
        public string? ApplicationVersion { get; set; }

        [JsonProperty("cn")]
        public string? ClientName { get; set; }

        [JsonProperty("cv")]
        public string? ClientVersion { get; set; }

        [JsonProperty("o")]
        public string? Other { get; set; }

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