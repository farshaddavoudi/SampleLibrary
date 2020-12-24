using ATA.Library.Shared.Service.Exceptions;
using Newtonsoft.Json;

namespace ATA.Library.Client.Dto.ApiResponseFormats
{
    public partial class ApiResult
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        //[System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
        //[JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("customStatusCode")]
        public ApiResultStatusCodeEnum CustomStatusCode { get; set; }

        [JsonProperty(PropertyName = "message", NullValueHandling = NullValueHandling.Ignore)]
        public string? Message { get; set; }

    }

    public partial class ApiResult<TData> : ApiResult
        where TData : class
    {
        [JsonProperty(propertyName: "data", NullValueHandling = NullValueHandling.Ignore)]
        public TData? Data { get; set; }
    }
}