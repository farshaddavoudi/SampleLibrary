using ATA.Library.Client.Service.HostServices.Book.Contracts;
using ATA.Library.Client.Service.HostServices.Category.Contracts;
using ATA.Library.Client.Web.Service.AppSetting;
using ATA.Library.Server.Model.Book;
using ATA.Library.Shared.Dto;
using ATA.Library.Shared.Service.Exceptions;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MimeTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.Pages
{
    public partial class BookForm
    {
        [Inject]
        private IBookHostService BookHostService { get; set; }

        [Inject]
        private ICategoryHostService CategoryHostService { get; set; }

        [Inject]
        private IToastService ToastService { get; set; }

        [Inject]
        private AppSettings AppSettings { get; set; }

        [Parameter]
        public int? BookId { get; set; }

        [Parameter]
        public string BookTitle { get; set; }

        private List<CategoryDto> _categories;

        BookDto _book = new BookDto();

        private string _coverImagePreview;

        protected override async Task OnInitializedAsync()
        {
            var categoriesResponse = await CategoryHostService.GetCategories();

            if (categoriesResponse == null)
            {
                var msg = "خطا در ارتباط با سرور";
                ToastService.ShowError(msg);
                throw new InvalidOperationException(msg);
            }

            if (!categoriesResponse!.IsSuccess)
            {
                ToastService.ShowError(categoriesResponse.Message);
                throw new InvalidOperationException(categoriesResponse.Message);
            }

            _categories = categoriesResponse.Data;

            if (_categories == null || _categories.Count == 0)
            {
                var errorMessage = "هیچ دسته‌ی مجازی برای شما وجود ندارد. با پشتیبانی تماس بگیرید";
                ToastService.ShowError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            if (BookId == null)
            { // Adding book
                _book.CategoryId = _categories.First().Id;
            }
            else
            { // Editing book
                // Get book from Db
                var getBookResult = await BookHostService.GetBookById((int)BookId);

                if (getBookResult == null)
                {
                    var msg = "ارتباط با سرور برقرار نشد. لطفا چند دقیقه بعد مجدد تلاش نمایید";
                    ToastService.ShowError(msg);
                    throw new BadRequestException(msg);
                }

                if (!getBookResult.IsSuccess)
                {
                    ToastService.ShowError(getBookResult.Message);
                    throw new BadRequestException(getBookResult.Message ?? "Operation failed");
                }

                _book = getBookResult.Data;
            }
        }

        private async Task HandleBookSubmit()
        {
            if (_book.BookFileByteData == null)
            {
                ToastService.ShowError("هیچ فایلی انتخاب نشده است");
                return;
            }

            if (_book.Id == default)
            { // Adding book
                var addResult = await BookHostService.AddBook(_book);

                if (addResult!.IsSuccess)
                {
                    ToastService.ShowSuccess("کتاب با موفقیت اضافه شد");

                    // todo: Modal to user for choose between
                    // 1- Another book add
                    // 2- Go to books
                }
                else
                {
                    ToastService.ShowError(addResult.Message);
                }
            }
            else
            { // Editing book

            }
        }

        private async Task OnCoverImageFileSelection(InputFileChangeEventArgs e)
        {
            var maxAllowedSize = AppSettings.FileUploadLimits!.MaxCoverImageSizeInKB * 1000;
            if (e.File.Size > maxAllowedSize)
            {
                ToastService.ShowError("حجم فایل بیشتر از مقدار مجاز می‌باشد");
                return;
            }

            var allowedMimeTypes = new List<string> { "image/jpeg", "image/png" };

            if (!allowedMimeTypes.Contains(e.File.ContentType))
            {
                ToastService.ShowError("نوع فایل عکس انتخابی درست نمی‌باشد. فقط عکس‌های jpg و png مجاز هستند.");
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
            var maxAllowedSize = AppSettings.FileUploadLimits!.MaxBookFileSizeInMB * 1000000;

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
