using ATA.Library.Client.Web.Service.Category.Contracts;
using ATA.Library.Shared.Dto;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.Shared
{
    public partial class NavMenu
    {
        private List<CategoryDto> _categories;

        private bool collapseNavMenu = true;

        private string NavMenuCssClass => collapseNavMenu ? "collapse" : null;


        [Inject]
        private ICategoryWebService CategoryWebService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _categories = await CategoryWebService.GetCategories();
        }

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}
