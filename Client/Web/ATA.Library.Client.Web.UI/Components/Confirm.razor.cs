using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.Components
{
    public partial class Confirm
    {
        [Parameter]
        public string ConfirmationMessage { get; set; } = "آیا مطمئن هستید؟";

        [CascadingParameter]
        private BlazoredModalInstance ConfirmModal { get; set; }

        private async Task OnConfirm()
        {
            await ConfirmModal.Close(ModalResult.Ok(true));
        }

        private async Task OnCancel()
        {
            await ConfirmModal.Cancel();
        }
    }
}
