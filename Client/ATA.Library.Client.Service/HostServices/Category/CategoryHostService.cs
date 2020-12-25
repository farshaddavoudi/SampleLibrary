using ATA.Library.Client.Service.HostServices.Category.Contracts;
using ATA.Library.Shared.Dto;
using System.Collections.Generic;
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
    }
}