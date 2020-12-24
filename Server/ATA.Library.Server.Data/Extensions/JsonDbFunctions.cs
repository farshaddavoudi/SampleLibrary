using ATA.Library.Shared.Service.Extensions;
using System.Dynamic;
using System.Linq;

namespace ATA.Library.Server.Data.Extensions
{
    public static class JsonDbFunctions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression">Db column value</param>
        /// <param name="path">Example: $.hn -> HostName JsonProperty field  </param>
        /// <returns></returns>
        public static string Value(string expression, string path)
        {
            // for UseInMemoryDatabase provider support

            var dynamicObject = expression.DeserializeToModel<ExpandoObject>();

            var jsonFieldName = path.Replace("$.", "");

            return dynamicObject.FirstOrDefault(p => p.Key == jsonFieldName).Value.ToString();

            //throw new InvalidOperationException($"{nameof(Value)}cannot be called client side");
        }
    }
}