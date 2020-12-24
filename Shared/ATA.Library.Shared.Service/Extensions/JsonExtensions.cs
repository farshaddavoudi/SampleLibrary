using ATA.Library.Shared.Service.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace ATA.Library.Shared.Service.Extensions
{
    public static class JsonExtensions
    {
        public static string? SerializeToJson(this object? objectModel, bool ignoreNullValues = false, bool indented = false)
        {
            if (objectModel == null)
                return null;

            try
            {
                var jsonSetting = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = ignoreNullValues ? NullValueHandling.Ignore : NullValueHandling.Include, //Default is Include
                    Formatting = indented ? Formatting.Indented : Formatting.None //Default is None
                };

                return JsonConvert.SerializeObject(objectModel, Formatting.None, jsonSetting);
            }
            catch (Exception ex)
            {
                throw new DomainLogicException(ex.Message);
            }
        }

        public static T DeserializeToModel<T>(this string json)
        {
            try
            {
                if (string.IsNullOrEmpty(json))
                    return default!;

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                return JsonConvert.DeserializeObject<T>(json, settings)!;
            }

            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}