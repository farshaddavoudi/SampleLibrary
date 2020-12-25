using ATA.Library.Shared.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATA.Library.Client.Service.HostServices.Category.Contracts
{
    public interface ICategoryHostService
    {
        Task<ApiResult<List<CategoryDto>>?> GetCategories();
    }
}