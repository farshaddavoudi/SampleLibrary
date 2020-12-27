using ATA.Library.Client.Service.HostServices.Category.Contracts;
using ATA.Library.Server.Model.Book;
using ATA.Library.Shared.Dto;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ATA.Library.Shared.Service.Extensions;

namespace ATA.Library.Client.Web.UI.Pages
{
    public partial class BookForm
    {
        [Inject]
        private ICategoryHostService CategoryHostService { get; set; }

        [Inject]
        private IToastService ToastService { get; set; }

        private List<CategoryDto> _categories;

        [Parameter]
        public int? BookId { get; set; } = 1;

        private BookDto _book = new BookDto();

        protected override async Task OnInitializedAsync()
        {
            var categoriesResponse = await CategoryHostService.GetCategories();

            if (categoriesResponse == null)
            {
                ToastService.ShowError("خطا در ارتباط با سرور");
                return;
            }

            if (!categoriesResponse!.IsSuccess)
            {
                ToastService.ShowError(categoriesResponse.Message);
                return;
            }

            _categories = categoriesResponse.Data;

            Console.WriteLine(_categories.SerializeToJson());

            if (_categories?.Count == 0)
            {
                var errorMessage = "هیچ دسته‌ی مجازی برای شما وجود ندارد. با پشتیبانی تماس بگیرید";
                ToastService.ShowError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            _book = new BookDto
            {
                CategoryId = 1,
                Title = "نام کتاب"
            };
        }

        private Task HandleBookSubmit()
        {
            throw new NotImplementedException();
        }
    }
}
