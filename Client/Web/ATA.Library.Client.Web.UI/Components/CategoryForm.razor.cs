using ATA.Library.Client.Service.HostServices.Category.Contracts;
using ATA.Library.Shared.Dto;
using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.Components
{
    public partial class CategoryForm
    {
        [Inject]
        private ICategoryHostService CategoryHostService { get; set; }

        [Inject]
        private IToastService ToastService { get; set; }

        [Parameter]
        public CategoryDto Category { get; set; } = new CategoryDto();

        [CascadingParameter]
        private BlazoredModalInstance CategoryModal { get; set; }

        private async Task HandleCategorySubmit()
        {
            if (Category.Id == default)
            { // Add
                // Save into Db
                var result = await CategoryHostService.AddCategory(Category);

                if (result!.IsSuccess)
                {
                    // Return to modal parent
                    await CategoryModal.Close(ModalResult.Ok(Category));
                }
                else
                {
                    ToastService.ShowError(result.Message);
                }
            }
            else
            { // Edit
                // Update Db
                var result = await CategoryHostService.EditCategory(Category);

                if (result != null && !result.IsSuccess)
                {
                    ToastService.ShowError(result.Message);
                    return;
                }

                // Return to modal parent
                await CategoryModal.Close(ModalResult.Ok(Category));
            }
        }

        private async Task CancelAndReturnBack()
        {
            await CategoryModal.Cancel();
        }
    }
}
