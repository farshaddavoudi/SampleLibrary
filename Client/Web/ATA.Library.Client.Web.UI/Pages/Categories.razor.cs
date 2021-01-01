using ATA.Library.Client.Web.Service.Category.Contracts;
using ATA.Library.Client.Web.UI.Components;
using ATA.Library.Shared.Dto;
using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using ATA.Library.Client.Web.UI.Shared;

namespace ATA.Library.Client.Web.UI.Pages
{
    public partial class Categories
    {
        private List<CategoryDto> _categories;

        [Inject]
        private ICategoryWebService CategoryWebService { get; set; }

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
            var confirmParams = new ModalParameters();

            confirmParams.Add(nameof(Confirm.ConfirmationMessage), $"آیا از حذف دسته {category.CategoryName} مطمئن می‌باشید؟");

            var confirmModal = ModalService.Show<Confirm>("تایید حذف", confirmParams);

            var result = await confirmModal.Result;

            if (!result.Cancelled && (bool)result.Data)
            {
                await CategoryWebService.DeleteCategory(category);

                ToastService.ShowSuccess("حذف دسته با موفقیت انجام شد");

                await LoadTableData();
            }
        }

        private async Task LoadTableData()
        {
            _categories = await CategoryWebService.GetCategories();

            StateHasChanged();
        }
    }
}
