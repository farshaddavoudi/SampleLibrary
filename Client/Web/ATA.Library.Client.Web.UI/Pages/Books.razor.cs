using ATA.Library.Client.Web.Service.Book.Contracts;
using ATA.Library.Client.Web.Service.Category.Contracts;
using ATA.Library.Server.Model.Book;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.Pages
{
    public partial class Books
    {
        private List<BookDto> _books;

        [Parameter]
        public int? CategoryId { get; set; }

        [Parameter]
        public string CategoryTitle { get; set; }

        [Inject]
        private ICategoryWebService CategoryWebService { get; set; }

        [Inject]
        private IBookWebService BookWebService { get; set; }

        [Inject]
        private IToastService ToastService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (CategoryId == null)
            { // Default category books
                var categories = await CategoryWebService.GetCategories();

                _books = await BookWebService.GetBooksByCategory(categories.First().Id);
            }
            else
            {
                _books = await BookWebService.GetBooksByCategory((int)CategoryId);
            }
        }
    }
}
