using ATA.Library.Shared.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.Service.Category.Contracts
{
    public interface ICategoryWebService
    {
        Task<List<CategoryDto>> GetCategories();

        Task AddCategory(CategoryDto category);

        Task EditCategory(CategoryDto category);

        Task DeleteCategory(CategoryDto category);
    }
}