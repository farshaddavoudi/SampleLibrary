using ATA.Library.Client.Web.Service.Category.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.Shared
{
    public partial class NavMenu
    {
        private string _categoriesAdminRoles;

        [Inject]
        private ICategoryWebService CategoryWebService { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            if (authState.User.Identity == null)
                return;

            if (authState.User.Identity.IsAuthenticated)
            {
                var categories = await CategoryWebService.GetCategories();

                //Administrator is super role which is not in categories
                var rolesCanSeeManageBook = new List<string> { "Administrator" };

                rolesCanSeeManageBook.AddRange(categories.Select(c => c.AdminRole));

                _categoriesAdminRoles = string.Join(",", rolesCanSeeManageBook);

            }
        }
    }
}
