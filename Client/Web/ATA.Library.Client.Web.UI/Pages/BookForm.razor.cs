using ATA.Library.Client.Web.Service.AppSetting;
using ATA.Library.Client.Web.Service.Book.Contracts;
using ATA.Library.Client.Web.Service.Category.Contracts;
using ATA.Library.Client.Web.Service.Enums;
using ATA.Library.Client.Web.UI.Extensions;
using ATA.Library.Shared.Core;
using ATA.Library.Shared.Dto;
using Blazored.Toast.Services;
using Humanizer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MimeTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.Pages
{
    public partial class BookForm
    {
        BookDto _book = new();

        private List<CategoryDto> _categories;

        private UploadStatus? _uploadStatus;

        private string _uploadStatusTitle;

        private bool _isSaving;

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

        [Inject]
        private HttpClient HostClient { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IWebAssemblyHostEnvironment HostEnvironment { get; set; }

        [Inject]
        private AppSettings AppSettings { get; set; }

        [Parameter]
        public int? BookId { get; set; }

        [Parameter]
        public string BookTitle { get; set; }


        private string _coverImagePreview;

        protected override async Task OnInitializedAsync()
        {
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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
                await JsRuntime.SetLayoutTitle("افزودن کتاب");

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task HandleBookSubmit()
        {
            _isSaving = true;

            if (_book.Id == default)
            { // Adding book
                await BookWebService.AddBook(_book);

                ToastService.ShowSuccess("کتاب با موفقیت اضافه شد");

                _coverImagePreview = null;

                var categoryId = _book.CategoryId;

                _book = new BookDto { CategoryId = categoryId };

            }
            else
            { // Editing book
                await BookWebService.EditBook(_book);

                ToastService.ShowSuccess("ویرایش کتاب با موفقیت انجام شد");
            }

            _isSaving = false;

            _uploadStatus = null;
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

            _uploadStatus = UploadStatus.Started;

            _uploadStatusTitle = $"Uploading {e.File.Size.Bytes().Humanize("0.0")} ...";

            var buffers = new byte[e.File.Size];

            _book.BookFileSize = e.File.Size;

            _book.BookFileFormat = MimeTypeMap.GetExtension(e.File.ContentType);

            var bookName = e.File.Name.Length > 50 ? e.File.Name.Substring(0, 50) : e.File.Name;

            using (var client = new HttpClient())
            {
                client.BaseAddress = HostClient.BaseAddress;
                var token = await JsRuntime.GetCookieAsync(AppStrings.ATAAuthTokenKey);
                client.DefaultRequestHeaders.Add(AppStrings.ATAAuthTokenKey, token);
                client.Timeout = TimeSpan.FromHours(1);

                await using (var fileStream = e.File.OpenReadStream(maxAllowedSize))
                {
                    await fileStream.ReadAsync(buffers);
                    var content = new MultipartFormDataContent { { new ByteArrayContent(buffers), "file", bookName.Replace(" ", "-") } };
                    _book.BookFileUrl = await BookWebService.UploadBookFile(content);
                    await fileStream.DisposeAsync();
                }
            }

            _uploadStatus = UploadStatus.Finished;

            _uploadStatusTitle = $"Successfully Uploaded";
        }

        private void Cancel()
        {
            NavigationManager.NavigateTo("/books");
        }

        private string GetCoverAbsoluteUrlFromCoverName(string coverName)
        {
            return HostEnvironment.IsDevelopment()
                ? AppSettings.BookBaseUrls!.CoverBaseUrl
                : $"{AppSettings.BookBaseUrls!.CoverBaseUrl}/{coverName}";
        }
    }
}
