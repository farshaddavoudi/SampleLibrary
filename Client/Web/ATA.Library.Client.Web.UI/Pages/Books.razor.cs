using ATA.Library.Client.Web.Service.Book.Contracts;
using ATA.Library.Client.Web.Service.Category.Contracts;
using ATA.Library.Server.Model.Book;
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
        private NavigationManager NavigationManager { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (CategoryId == null)
            {
                // Default category books
                var categories = await CategoryWebService.GetCategories();

                var defaultCategory = categories.First();

                NavigationManager.NavigateTo($"books/{defaultCategory.Id}/{defaultCategory.CategoryName?.Replace(" ", "-")}");
            }
            else
            {
                _books = await BookWebService.GetBooksByCategory((int)CategoryId);
            }
        }
    }
}