using ATA.Library.Client.Service.HostServices.Category.Contracts;
using ATA.Library.Client.Web.UI.Components;
using ATA.Library.Shared.Dto;
using Blazored.Modal;
using Blazored.Modal.Services;
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

        [CascadingParameter]
        private IModalService ModalService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadTableData();
        }

        private async Task OnAddCategory()
        {
            var categoryForm = ModalService.Show<CategoryForm>("افزودن دسته");

            var result = await categoryForm.Result;

            if (!result.Cancelled)
            {
                ToastService.ShowSuccess("افزودن دسته با موفقیت انجام شد");

                await LoadTableData();
            }
        }

        private async Task OnEditCategory(CategoryDto category)
        {
            var categoryParam = new ModalParameters();

            categoryParam.Add(nameof(CategoryForm.Category), category);

            var categoryForm = ModalService.Show<CategoryForm>("ویرایش دسته", categoryParam);

            var result = await categoryForm.Result;

            if (!result.Cancelled)
            {
                ToastService.ShowSuccess("ویرایش دسته با موفقیت انجام شد");

                await LoadTableData();
            }
        }

        private async Task OnDeleteCategory(CategoryDto category)
        {
            throw new System.NotImplementedException();
        }

        private async Task LoadTableData()
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
    }
}
