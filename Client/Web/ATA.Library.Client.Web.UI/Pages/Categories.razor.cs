using ATA.Library.Client.Service.HostServices.Category.Contracts;
using ATA.Library.Shared.Dto;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.Pages
{
    public partial class Categories
    {
        private List<CategoryDto> _categories;

        [Inject]
        private ICategoryHostService CategoryHostService { get; set; }

        [Inject]
        private IToastService ToastService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var hostResponse = await CategoryHostService.GetCategories();

            if (hostResponse == null)
            {
                ToastService.ShowError("خطایی رخ داده است. هیچ پاسخی از سمت سرور یافت نشد");
                return;
            }


            if (!hostResponse.IsSuccess)
            {
                ToastService.ShowError(hostResponse.Message);
                return;
            }

            _categories = hostResponse.Data;
        }

        private async Task OnAddCategory()
        {
            ToastService.ShowSuccess("افزودن دسته با موفقیت انجام شد");
        }
    }
}
