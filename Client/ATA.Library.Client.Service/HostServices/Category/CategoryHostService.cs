using ATA.Library.Client.Service.HostServices.Category.Contracts;
using ATA.Library.Shared.Dto;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ATA.Library.Client.Service.HostServices.Category
{
    public class CategoryHostService : ICategoryHostService
    {
        private readonly HttpClient _hostClient;

        #region Constructor Injections

        public CategoryHostService(HttpClient hostClient)
        {
            _hostClient = hostClient;
        }

        #endregion

        public async Task<ApiResult<List<CategoryDto>>?> GetCategories()
        {
            return await _hostClient.GetFromJsonAsync<ApiResult<List<CategoryDto>>>("api/v1/category/get-all");
        }

        public async Task<ApiResult?> AddCategory(CategoryDto category)
        {
            var httpResponseMessage = await _hostClient.PostAsJsonAsync("api/v1/category/add", category);

            return await httpResponseMessage.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ApiResult?> EditCategory(CategoryDto category)
        {
            var httpResponseMessage =
                await _hostClient.PutAsJsonAsync($"api/v1/category/edit?categoryId={category.Id}", category);

            if (httpResponseMessage.StatusCode != HttpStatusCode.NoContent)
                return await httpResponseMessage.Content.ReadFromJsonAsync<ApiResult?>();

            return new ApiResult { IsSuccess = true };
        }

        public async Task<ApiResult?> DeleteCategory(CategoryDto category)
        {
            var httpResponseMessage = await _hostClient.DeleteAsync($"api/v1/category/delete?categoryId={category.Id}");

            if (httpResponseMessage.StatusCode != HttpStatusCode.NoContent)
                return await httpResponseMessage.Content.ReadFromJsonAsync<ApiResult?>();

            return new ApiResult { IsSuccess = true };
        }
    }
}