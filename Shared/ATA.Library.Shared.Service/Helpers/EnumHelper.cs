using ATA.Library.Shared.Core.CoreModels;
using ATA.Library.Shared.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATA.Library.Shared.Service.Helpers
{
    public static class EnumHelper
    {
        /// <summary>
        /// Get List of Enum All Values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetEnumValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Get List of SelectListItem from Enum. Item(s) Text is set on Enum DisplayName property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<SelectListItem> GetEnumSelectList<T>() where T : System.Enum
        {
            return GetEnumValues<T>().Select(e => new SelectListItem(
                e.ToDisplayName(), e.ToString("d"))).ToList();
        }

        /// <summary>
        /// Get Dictionary from Enum. Dictionary value is set on Enum DisplayName property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<int, string?> GetEnumDictionary<T>() where T : System.Enum
        {
            return GetEnumValues<T>().ToDictionary(p => Convert.ToInt32(p), q => q.ToDisplayName());
        }

    }
}