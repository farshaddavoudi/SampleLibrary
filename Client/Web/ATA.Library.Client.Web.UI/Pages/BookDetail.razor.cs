using ATA.Library.Client.Web.Service.Book.Contracts;
using ATA.Library.Client.Web.UI.Extensions;
using ATA.Library.Server.Model.Book;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.Pages
{
    public partial class BookDetail
    {
        private BookDto _book;

        [Parameter]
        public int BookId { get; set; }

        [Parameter]
        public string BookTitle { get; set; }

        [Inject]
        private IBookWebService BookWebService { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _book = await BookWebService.GetBookById(BookId);
        }

        private async Task ShowBook()
        {
            await JsRuntime.NavigateToUrlInNewTab($"/book-viewer/{BookId}/{_book.Title?.Replace(" ", "-")}");
        }
    }
}
