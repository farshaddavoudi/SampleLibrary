using ATA.Core.Information;
using ATA.Library.Server.Api.ActionFilters;
using ATA.Library.Server.Model.HttpTypedClients.ATASecurityClient;
using ATA.Library.Server.Service.Contracts;
using ATA.Library.Shared.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ATA.Library.Server.Api.Controllers.api.ATASecurity
{
    /// <summary>
    /// Get all Controllers and Actions in project used for Security project in ATA
    /// </summary>
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]// api/v1/[controller]
    [ApiController]
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class ApplicationInfoesController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _securityClient;
        public ApplicationInfoesController(IHttpClientFactory httpClientFactory,
            ATASecurityClient securityClient,
            IUserInfoProvider userInfoProvider)
        {
            _httpClientFactory = httpClientFactory;
            _securityClient = securityClient.Client;
            _securityClient.DefaultRequestHeaders.Add("SSOToken",
                userInfoProvider.CurrentUser()?.SsoToken);

        }


        [HttpGet("GetControllersInfoes")]
        [HttpPost("GetControllersInfoes")]

        public List<Core.Information.ApplicationInfo.ControllerActoin> GetControllersInfoes()
        {
            return Core.Information.ApplicationInfo.GetControllersInfoes();
        }

        [HttpGet("GetPagesInfoes")]
        [HttpPost("GetPagesInfoes")]
        public async Task<List<ApplicationInfo.PageInfo>> GetPagesInfoes()
        {
            var uiClient = _httpClientFactory.CreateClient(AppStrings.ATASecurityAppKeyName);

            var httpResponseMessage = await uiClient.GetAsync("cats.json")
                .ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());
            var result = await httpResponseMessage.Content.ReadFromJsonAsync<CategoriesModel>();
            var methodResponse = new List<ApplicationInfo.PageInfo>();
            foreach (var item in result?.Categories!)
            {
                foreach (var page in item.Pages!)
                {
                    methodResponse.Add(new ApplicationInfo.PageInfo
                    {
                        Title = page.Title,
                        PageClassName = $"{item.Name}.{page.Name}",
                        Remarks = page.Remark
                    });
                }
            }
            return methodResponse;
        }

        /// <summary>
        /// Get all user allowed pages and modules
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAccessiblePages")]
        [ApiResultFilter]
        public async Task<List<AccessiblePageInfo>?> GetAccessiblePages()
        {
            var httpResponseMessage = await _securityClient.GetAsync(
                "api/Authentication/GetAccessiblePages?appName=performanceevaluation"
            ).ContinueWith(postTask => postTask.Result.EnsureSuccessStatusCode());

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<GetAccessiblePagesResponseModel>();
            return response?.Data;
        }

        #region GetPagesInfoes Classes
        internal class CategoriesModel
        {
            public List<Category>? Categories { get; set; }
        }

        internal class Category
        {
            public string? Name { get; set; }
            public List<Page>? Pages { get; set; }
        }
        internal class Page
        {
            public string? Name { get; set; }
            public string? Title { get; set; }
            public string? Remark { get; set; }
        }
        #endregion

        #region GetAccessiblePages
        public class GetAccessiblePagesResponseModel
        {
            public List<AccessiblePageInfo>? Data { get; set; }
            public bool IsSuccessful { get; set; }
            public string? Message { get; set; }
        }
        public class AccessiblePageInfo
        {
            public int ApplicationPageID { get; set; }
            public string? ClassName { get; set; }
            public string? Title { get; set; }
            public string? Remarks { get; set; }
            public bool Anonymous { get; set; }
        }
        #endregion
    }



}
