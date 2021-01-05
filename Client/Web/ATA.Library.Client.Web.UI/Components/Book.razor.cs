using ATA.Library.Client.Web.Service.AppSetting;
using ATA.Library.Client.Web.Service.Book.Contracts;
using ATA.Library.Client.Web.UI.Extensions;
using ATA.Library.Shared.Core;
using ATA.Library.Shared.Dto;
using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.Components
{
    public partial class Book
    {
        private string _coverImageUrl;

        private string _rolesCanManageBook;

        [Parameter]
        public BookDto BookDto { get; set; }

        [Parameter]
        public EventCallback StateChangeRequest { get; set; }

        [CascadingParameter]
        private IModalService ModalService { get; set; }

        [Inject]
        private AppSettings AppSettings { get; set; }

        [Inject]
        private IWebAssemblyHostEnvironment HostEnvironment { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IBookWebService BookWebService { get; set; }

        [Inject]
        private IToastService ToastService { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        protected override void OnParametersSet()
        {
            _coverImageUrl = HostEnvironment.IsDevelopment()
                ? AppSettings.BookBaseUrls!.CoverBaseUrl
                : $"{AppSettings.BookBaseUrls!.CoverBaseUrl}/{BookDto.CoverImageUrl}";

            _rolesCanManageBook = $"{AppStrings.Claims.Administrator},{BookDto.Category!.AdminRole}";

            base.OnParametersSet();
        }

        private void EditBook()
        {
            NavigationManager.NavigateTo($"/book-form/{BookDto.Id}/{BookDto.Title?.Replace(" ", "-")}");
        }

        private async Task DeleteBook()
        {
            var confirmParams = new ModalParameters();

            confirmParams.Add(nameof(Confirm.ConfirmationMessage), $"آیا از حذف کتاب {BookDto.Title} مطمئن می‌باشید؟");

            var confirmModal = ModalService.Show<Confirm>("تایید حذف", confirmParams);

            var result = await confirmModal.Result;

            if (!result.Cancelled && (bool)result.Data)
            {
                await BookWebService.DeleteBook(BookDto.Id);

                ToastService.ShowSuccess("حذف کتاب با موفقیت انجام شد");

                await StateChangeRequest.InvokeAsync();
            }
        }

        private async Task ViewOrDownloadBook()
        {
            if (BookDto.IsDownloadable)
            {
                var fileUrl = HostEnvironment.IsDevelopment()
                    ? AppSettings.BookBaseUrls!.FileBaseUrl
                    : $"{AppSettings.BookBaseUrls!.FileBaseUrl}/{BookDto.BookFileUrl}";

                await JsRuntime.NavigateToUrlInNewTab(fileUrl);
            }
            else
            {
                await JsRuntime.NavigateToUrlInNewTab($"/book-viewer/{BookDto.Id}/{BookDto.Title?.Replace(" ", "-")}");
            }
        }
    }
}
