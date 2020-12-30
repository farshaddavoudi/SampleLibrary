using Microsoft.AspNetCore.Components;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.Pages
{
    public partial class BookViewer
    {
        public string DocumentPath { get; set; }

        [Parameter] public int BookId { get; set; }

        [Parameter] public string BookTitle { get; set; }

        protected override async Task OnInitializedAsync()
        {
            string url = "https://cdn.app.ataair.ir/portal/library/education/DIRTY%20DOZEN%20-FC.pdf";

            byte[] byteArray;

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                await using (Stream fileSourceStream = await response.Content.ReadAsStreamAsync())
                {
                    await using (var memoryStream = new MemoryStream())
                    {
                        await fileSourceStream.CopyToAsync(memoryStream);
                        byteArray = memoryStream.ToArray();
                    }
                }
            }

            DocumentPath = "data:application/pdf;base64," + Convert.ToBase64String(byteArray);
        }
    }
}
