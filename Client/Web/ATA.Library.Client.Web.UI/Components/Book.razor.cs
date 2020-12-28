using ATA.Library.Client.Web.Service.AppSetting;
using ATA.Library.Server.Model.Book;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ATA.Library.Client.Web.UI.Components
{
    public partial class Book
    {
        private string _coverImageUrl;

        [Parameter]
        public BookDto BookDto { get; set; }

        [Inject]
        private AppSettings AppSettings { get; set; }

        [Inject]
        private IWebAssemblyHostEnvironment HostEnvironment { get; set; }

        protected override void OnParametersSet()
        {
            _coverImageUrl = HostEnvironment.IsDevelopment()
                ? AppSettings.BookBaseUrls!.CoverBaseUrl
                : $"{AppSettings.BookBaseUrls!.CoverBaseUrl}/{BookDto.CoverImageUrl}";
        }
    }
}
