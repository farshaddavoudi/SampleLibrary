using ATA.Library.Client.Web.Service.AppSetting;
using ATA.Library.Client.Web.Service.Book.Contracts;
using ATA.Library.Client.Web.UI.Extensions;
using ATA.Library.Shared.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.Pages
{
    public partial class BookDetail
    {
        private string _coverImageUrl;

        private BookDto _book;

        [Parameter]
        public int BookId { get; set; }

        [Parameter]
        public string BookTitle { get; set; }

        [Inject]
        private IBookWebService BookWebService { get; set; }

        [Inject]
        private AppSettings AppSettings { get; set; }

        [Inject]
        private IWebAssemblyHostEnvironment HostEnvironment { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _book = await BookWebService.GetBookById(BookId);
        }

        protected override void OnParametersSet()
        {
            _coverImageUrl = HostEnvironment.IsDevelopment()
                ? AppSettings.BookBaseUrls!.CoverBaseUrl
                : $"{AppSettings.BookBaseUrls!.CoverBaseUrl}/{_book.CoverImageUrl}";
        }

        private async Task ShowBook()
        {
            NavigationManager.NavigateTo($"/book-viewer/{BookId}/{_book.Title?.Replace(" ", "-")}");
        }

        private async Task DownloadBook()
        {
            var fileUrl = HostEnvironment.IsDevelopment()
                ? AppSettings.BookBaseUrls!.FileBaseUrl
                : $"{AppSettings.BookBaseUrls!.FileBaseUrl}/{_book.BookFileUrl}";

            await JsRuntime.NavigateToUrlInNewTab(fileUrl);

            //NavigationManager.NavigateTo(fileUrl!);
        }
    }
}
