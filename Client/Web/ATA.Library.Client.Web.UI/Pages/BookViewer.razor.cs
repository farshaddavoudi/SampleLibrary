using ATA.Library.Client.Web.Service.AppSetting;
using Microsoft.AspNetCore.Components;

namespace ATA.Library.Client.Web.UI.Pages
{
    public partial class BookViewer
    {
        private string _serverUrl;
        [Parameter] public int? BookId { get; set; }

        [Parameter] public string BookTitle { get; set; }

        [Inject] private AppSettings AppSettings { get; set; }

        protected override void OnInitialized()
        {
            _serverUrl = $"{AppSettings.Urls!.HostUrl}api/v1/pdf-viewer";
            base.OnInitialized();
        }
    }
}
