using ATA.Library.Shared.Dto;
using System.Collections.Generic;

namespace ATA.Library.Client.Web.Service
{
    public class AppData
    {
        public List<CategoryDto> Categories { get; set; } = new();
    }
}