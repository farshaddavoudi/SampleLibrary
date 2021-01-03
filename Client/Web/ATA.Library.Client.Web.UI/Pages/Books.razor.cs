using ATA.Library.Client.Web.Service.Book.Contracts;
using ATA.Library.Client.Web.Service.Category.Contracts;
using ATA.Library.Shared.Dto;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.Pages
{
    public partial class Books
    {
        private List<CategoryDto> _categories;

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

        [Inject]
        private IToastService ToastService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            _categories = await CategoryWebService.GetCategories();

            if (_categories.Count == 0)
                return;

            if (CategoryId == null)
            {
                var defaultCategory = _categories.First();

                CategoryId = defaultCategory.Id;

                NavigationManager.NavigateTo($"books/{CategoryId}/{defaultCategory.CategoryName?.Replace(" ", "-")}");
            }
            else
            {
                _books = await BookWebService.GetBooksByCategory((int)CategoryId);
            }
        }

        private async Task StateChangeRequested()
        {
            _books = await BookWebService.GetBooksByCategory((int)CategoryId!);

            StateHasChanged();
        }
    }
}