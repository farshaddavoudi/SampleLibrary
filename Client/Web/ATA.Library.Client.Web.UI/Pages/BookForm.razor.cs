using ATA.Library.Client.Web.Service.Book.Contracts;
using ATA.Library.Client.Web.Service.Category.Contracts;
using ATA.Library.Client.Web.UI.Extensions;
using ATA.Library.Shared.Core;
using ATA.Library.Shared.Dto;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MimeTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.Pages
{
    public partial class BookForm
    {
        private List<CategoryDto> _categories;

        private bool _showUploadLoading;

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        private IBookWebService BookWebService { get; set; }

        [Inject]
        private ICategoryWebService CategoryWebService { get; set; }

        [Inject]
        private IToastService ToastService { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        [Parameter]
        public int? BookId { get; set; }

        [Parameter]
        public string BookTitle { get; set; }


        BookDto _book = new BookDto();

        private string _coverImagePreview;

        protected override async Task OnInitializedAsync()
        {
            await JsRuntime.SetLayoutTitle("افزودن کتاب");

            _categories = await CategoryWebService.GetCategories();

            var authState = await AuthenticationStateTask;

            var userRoles = authState.User.Claims.Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value).ToList();

            if (!userRoles.Contains(AppStrings.Claims.Administrator))
            {
                // Check which categories contain userRoles
                var allowedCategories = new List<CategoryDto>();

                foreach (var category in _categories)
                {
                    if (userRoles.Contains(category.AdminRole))
                        allowedCategories.Add(category);
                }

                _categories = allowedCategories;
            }

            if (BookId == null)
            { // Adding book
                _book.CategoryId = _categories.First().Id;
            }
            else
            { // Editing book
                // Get book from Db
                _book = await BookWebService.GetBookById((int)BookId);
            }
        }

        private async Task HandleBookSubmit()
        {
            if (_book.BookFileByteData == null)
            {
                ToastService.ShowError("هیچ فایلی انتخاب نشده است");
                return;
            }

            // Start Loading
            _showUploadLoading = true;
            StateHasChanged();

            if (_book.Id == default)
            { // Adding book
                await BookWebService.AddBook(_book);

                ToastService.ShowSuccess("کتاب با موفقیت اضافه شد");

                // todo: Modal to user for choose between
                // 1- Another book add
                // 2- Go to books

            }
            else
            { // Editing book

            }

            // Start Loading
            _showUploadLoading = false;
        }

        private async Task OnCoverImageFileSelection(InputFileChangeEventArgs e)
        {
            var allowedMimeTypes = new List<string> { "image/jpeg", "image/png" };

            if (!allowedMimeTypes.Contains(e.File.ContentType))
            {
                ToastService.ShowError("نوع فایل عکس انتخابی درست نمی‌باشد. فقط عکس‌های jpg و png مجاز هستند.");
                return;
            }

            var maxAllowedSize = AppStrings.UploadLimits.MaxCoverImageSizeInKB * 1000;

            if (e.File.Size > maxAllowedSize)
            {
                ToastService.ShowError("حجم فایل بیشتر از مقدار مجاز می‌باشد");
                return;
            }

            IBrowserFile coverImgFile = e.File;

            var buffers = new byte[coverImgFile.Size];

            await coverImgFile.OpenReadStream(maxAllowedSize).ReadAsync(buffers);

            string imgMimeType = coverImgFile.ContentType;

            _coverImagePreview = $"data:{imgMimeType};base64,{Convert.ToBase64String(buffers)}";

            _book.CoverImageByteData = buffers;

            _book.CoverImageFileFormat = MimeTypeMap.GetExtension(imgMimeType);
        }

        private async Task OnBookFileSelection(InputFileChangeEventArgs e)
        {
            var maxAllowedSize = AppStrings.UploadLimits.MaxBookFileSizeInMB * 1000000;

            if (e.File.Size > maxAllowedSize)
            {
                ToastService.ShowError("حجم فایل بیشتر از مقدار مجاز می‌باشد");
                return;
            }

            if (e.File.ContentType != "application/pdf")
            {
                ToastService.ShowError("فقط فایل‌های pdf امکان آپلود دارند.");
                return;
            }

            IBrowserFile bookFile = e.File;

            var buffers = new byte[bookFile.Size];

            await bookFile.OpenReadStream(maxAllowedSize).ReadAsync(buffers);

            _book.BookFileSize = bookFile.Size;

            _book.BookFileByteData = buffers;

            _book.BookFileFormat = MimeTypeMap.GetExtension(bookFile.ContentType);
        }
    }
}
