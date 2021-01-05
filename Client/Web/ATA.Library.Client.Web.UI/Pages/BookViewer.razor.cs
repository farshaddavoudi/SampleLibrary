using Microsoft.AspNetCore.Components;

namespace ATA.Library.Client.Web.UI.Pages
{
    public partial class BookViewer
    {
        [Parameter] public int? BookId { get; set; }

        [Parameter] public string BookTitle { get; set; }
    }
}
