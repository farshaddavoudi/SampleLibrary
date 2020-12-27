using ATA.Library.Client.Service.HostServices.Category.Contracts;
using ATA.Library.Client.Web.Service.AppSetting;
using ATA.Library.Server.Model.Book;
using ATA.Library.Server.Model.Enums;
using ATA.Library.Shared.Dto;
using ATA.Library.Shared.Service.Extensions;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.Pages
{
    public partial class BookForm
    {
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
                // ...
            }
        }

        private async Task HandleBookSubmit()
        {
            if (_book.FileData.Any(f => f.FileType == FileType.BookPdf) == false)
            {
                ToastService.ShowError("هیچ فایلی انتخاب نشده است");
                return;
            }

            Console.WriteLine(_book.SerializeToJson());
        }

        private async Task OnCoverImageFileSelection(InputFileChangeEventArgs e)
        {
            if (e.File.Size > AppSettings.FileUploadLimits!.MaxCoverImageSizeInKB * 1000)
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

            await coverImgFile.OpenReadStream().ReadAsync(buffers);

            string imgMimeType = coverImgFile.ContentType;

            _coverImagePreview = $"data:{imgMimeType};base64,{Convert.ToBase64String(buffers)}";

            if (_book.FileData.Any(f => f.FileType == FileType.CoverImage))
                _book.FileData.Remove(_book.FileData.First(f => f.FileType == FileType.CoverImage));

            _book.FileData.Add(new FileData
            {
                FileType = FileType.CoverImage,
                Data = buffers,
                MimeType = imgMimeType,
                Size = coverImgFile.Size
            });
        }

        private async Task OnBookFileSelection(InputFileChangeEventArgs e)
        {
            if (e.File.Size > AppSettings.FileUploadLimits!.MaxBookFileSizeInMB * 1000000)
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

            await bookFile.OpenReadStream().ReadAsync(buffers);

            string fileMimeType = bookFile.ContentType;

            if (_book.FileData.Any(f => f.FileType == FileType.BookPdf))
                _book.FileData.Remove(_book.FileData.First(f => f.FileType == FileType.BookPdf));

            _book.FileData.Add(new FileData
            {
                FileType = FileType.BookPdf,
                Data = buffers,
                MimeType = fileMimeType,
                Size = bookFile.Size
            });
        }
    }
}
