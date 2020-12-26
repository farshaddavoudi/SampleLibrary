using ATA.Library.Shared.Dto;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.Components
{
    public partial class CategoryForm
    {
        [Parameter]
        public CategoryDto Category { get; set; } = new CategoryDto();

        [CascadingParameter]
        private BlazoredModalInstance CategoryModal { get; set; }

        private async Task HandleCategorySubmit()
        {
            if (Category.Id == default)
            { // Create
                // Save into Db

                // Return to modal parent
                await CategoryModal.Close(ModalResult.Ok(Category));
            }
            else
            { // Edit
                // Update Db

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
