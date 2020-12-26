using ATA.Library.Shared.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATA.Library.Client.Service.HostServices.Category.Contracts
{
    public interface ICategoryHostService
    {
        Task<ApiResult<List<CategoryDto>>?> GetCategories();

        Task<ApiResult?> AddCategory(CategoryDto category);

        Task<ApiResult?> EditCategory(CategoryDto category);

        Task<ApiResult?> DeleteCategory(CategoryDto category);
    }
}