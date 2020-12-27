using ATA.Library.Client.Web.Service.Category.Contracts;
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
        private ICategoryWebService CategoryWebService { get; set; }

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
                await CategoryWebService.AddCategory(Category);

                // Return to modal parent
                await CategoryModal.Close(ModalResult.Ok(Category));
            }
            else
            { // Edit
                // Update Db
                await CategoryWebService.EditCategory(Category);

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
