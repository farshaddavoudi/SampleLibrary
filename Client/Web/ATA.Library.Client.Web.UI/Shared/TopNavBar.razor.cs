using ATA.Library.Client.Web.Service.AppSetting;
using ATA.Library.Client.Web.UI.Extensions;
using ATA.Library.Shared.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.Shared
{
    public partial class TopNavBar
    {
        private string _userDropdownCssClass;
        private string _userName;
        private string _personnelCode;
        private string _userProfileImageAddress;

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        [Inject]
        private HttpClient HostClient { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private AppSettings AppSettings { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            _userName = authState.User.Identity!.Name;

            var userPersonnelCodeClaim = authState.User.Claims.Single(c => c.Type == AppStrings.Claims.PersonnelCode);

            _personnelCode = userPersonnelCodeClaim.Value;

            _userProfileImageAddress = $"http://cdn.app.ataair.ir/img/pers/{_personnelCode}.png";

        }

        private void OnUserProfileClick()
        {
            _userDropdownCssClass = string.IsNullOrWhiteSpace(_userDropdownCssClass) ? "open" : string.Empty;
        }

        private void OnUserImageLoadFailed()
        {
            _userProfileImageAddress = "/images/ata-layout/user-default.jpg";
        }

        private async Task Logout()
        {
            await JsRuntime.DeleteCookieAsync(AppStrings.ATAAuthTokenKey); // workaround: not working
            HostClient.DefaultRequestHeaders.Remove(AppStrings.ATAAuthTokenKey);
            NavigationManager.NavigateTo($"http://security.app.ataair.ir/Application/Login.aspx?ReturnUrl={AppSettings.Urls!.AppAddress}", true);
        }

    }
}
