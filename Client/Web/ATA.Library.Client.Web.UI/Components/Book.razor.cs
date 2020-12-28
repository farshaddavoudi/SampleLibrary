using ATA.Library.Server.Model.Book;
using Microsoft.AspNetCore.Components;

namespace ATA.Library.Client.Web.UI.Components
{
    public partial class Book
    {
        [Parameter]
        public BookDto BookDto { get; set; }


    }
}
