using ATA.Library.Server.Service.Book.Contracts;
using ATA.Library.Shared.Service.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Syncfusion.EJ2.PdfViewer;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ATA.Library.Server.Api.Controllers.api.Syncfusion
{
    /// <summary>
    /// PdfViewer Service from Syncfusion (Refactored).
    /// Source from: https://www.syncfusion.com/kb/10346/how-to-create-pdf-viewer-web-service-application-in-asp-net-core
    /// </summary>

    [Route("api/v{version:apiVersion}/[controller]")]// api/v1/[controller]
    [ApiController]
    [ApiVersion("1")]
    [AllowAnonymous]
    public class PdfViewerController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IBookService _bookService;
        //Initialize the memory cache object   
        public IMemoryCache Cache;

        #region Constructor Injections

        public PdfViewerController(IWebHostEnvironment webHostEnvironment, IMemoryCache cache, IBookService bookService)
        {
            _webHostEnvironment = webHostEnvironment;
            Cache = cache;
            _bookService = bookService;
        }

        #endregion

        /// <summary>
        /// Post action for Loading the PDF documents   
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        [HttpPost("Load")]
        [AcceptVerbs("Post")]
        public async Task<IActionResult> Load([FromBody] Dictionary<string, string> jsonObject, CancellationToken cancellationToken)
        {
            //Initialize the PDF viewer object with memory cache object
            PdfRenderer pdfViewer = new PdfRenderer(Cache);

            MemoryStream stream;

            using (var client = new WebClient())
            {
                var bookAbsoluteUrl =
                    await _bookService.GetBookFileAbsoluteUrl(jsonObject["document"].ToInt(), cancellationToken);

                var bookFileBytes = await client.DownloadDataTaskAsync(bookAbsoluteUrl);

                stream = new MemoryStream(bookFileBytes);
            }

            var jsonResult = pdfViewer.Load(stream, jsonObject);

            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        /// <summary>
        /// Post action for processing the bookmarks from the PDF documents
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        [HttpPost("Bookmarks")]
        [AcceptVerbs("Post")]
        public IActionResult Bookmarks([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfViewer = new PdfRenderer(Cache);
            var jsonResult = pdfViewer.GetBookmarks(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        /// <summary>
        /// Post action for processing the PDF documents  
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        [HttpPost("RenderPdfPages")]
        [AcceptVerbs("Post")]
        public IActionResult RenderPdfPages([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfViewer = new PdfRenderer(Cache);
            object jsonResult = pdfViewer.GetPage(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        /// <summary>
        /// Post action for rendering the ThumbnailImages
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        [HttpPost("RenderThumbnailImages")]
        [AcceptVerbs("Post")]
        public IActionResult RenderThumbnailImages([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfViewer = new PdfRenderer(Cache);
            object result = pdfViewer.GetThumbnailImages(jsonObject);
            return Content(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// Post action for rendering the annotations
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        [HttpPost("RenderAnnotationComments")]
        [AcceptVerbs("Post")]
        public IActionResult RenderAnnotationComments([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfViewer = new PdfRenderer(Cache);
            object jsonResult = pdfViewer.GetAnnotationComments(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        /// <summary>
        /// Post action to export annotations
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        [HttpPost("ExportAnnotations")]
        [AcceptVerbs("Post")]
        public IActionResult ExportAnnotations([FromBody] Dictionary<string, string> jsonObject)
        {
            PdfRenderer pdfViewer = new PdfRenderer(Cache);
            string? jsonResult = pdfViewer.GetAnnotationComments(jsonObject).SerializeToJson();
            return Content(jsonResult);
        }

        /// <summary>
        /// Post action for unloading and disposing the PDF document resources  
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        [HttpPost("Unload")]
        [AcceptVerbs("Post")]
        public IActionResult Unload([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfViewer = new PdfRenderer(Cache);
            pdfViewer.ClearCache(jsonObject);
            return Content("Document cache is cleared");
        }

        /// <summary>
        /// Post action for downloading the PDF documents
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        [HttpPost("Download")]
        public IActionResult Download([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfViewer = new PdfRenderer(Cache);
            string documentBase = pdfViewer.GetDocumentAsBase64(jsonObject);
            return Content(documentBase);
        }

        /// <summary>
        /// Post action for printing the PDF documents
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        [HttpPost("PrintImages")]
        public IActionResult PrintImages([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfViewer = new PdfRenderer(Cache);
            object pageImage = pdfViewer.GetPrintImage(jsonObject);
            return Content(JsonConvert.SerializeObject(pageImage));
        }
    }
}