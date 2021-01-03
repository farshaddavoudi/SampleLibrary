using ATA.Library.Shared.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.Shared
{
    public partial class TopNavBar
    {
        private string _userDropdownCssClass;
        private string _userName;
        private string _userProfileImageAddress;

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            _userName = authState.User.Identity!.Name;

            var userPersonnelCodeClaim = authState.User.Claims.Single(c => c.Type == AppStrings.Claims.PersonnelCode);

            var personnelCode = userPersonnelCodeClaim.Value;

            _userProfileImageAddress = $"http://cdn.app.ataair.ir/img/pers/{personnelCode}.png";

        }

        private void OnUserProfileClick()
        {
            if (string.IsNullOrWhiteSpace(_userDropdownCssClass))
            {
                _userDropdownCssClass = "open";
            }
            else
            {
                _userDropdownCssClass = string.Empty;
            }
        }

        private void OnUserImageLoadFailed()
        {
            _userProfileImageAddress = "/images/ata-layout/user-default.jpg";
        }
    }
}
