using ATA.Library.Shared.Service.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Syncfusion.EJ2.PdfViewer;
using System;
using System.Collections.Generic;
using System.IO;

namespace ATA.Library.Server.Api.Controllers.api.Syncfusion
{
    /// <summary>
    /// PdfViewer Service from Syncfusion.
    /// Copied from: https://www.syncfusion.com/kb/10346/how-to-create-pdf-viewer-web-service-application-in-asp-net-core
    /// </summary>

    [Route("api/v{version:apiVersion}/[controller]")]// api/v1/[controller]
    [ApiController]
    [ApiVersion("1")]
    public class PdfViewerController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        //Initialize the memory cache object   
        public IMemoryCache Cache;

        public PdfViewerController(IWebHostEnvironment webHostEnvironment, IMemoryCache cache)
        {
            _webHostEnvironment = webHostEnvironment;
            Cache = cache;
        }

        /// <summary>
        /// Post action for Loading the PDF documents   
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        [HttpPost("Load")]
        [AcceptVerbs("Post")]
        public IActionResult Load([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF viewer object with memory cache object
            PdfRenderer pdfviewer = new PdfRenderer(Cache);
            MemoryStream stream = new MemoryStream();
            object jsonResult = new object();
            if (jsonObject != null && jsonObject.ContainsKey("document"))
            {
                if (bool.Parse(jsonObject["isFileName"]))
                {
                    //string documentPath = GetDocumentPath(jsonObject["document"]);
                    string documentPath = "text-assing";
                    if (!string.IsNullOrEmpty(documentPath))
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(documentPath);
                        stream = new MemoryStream(bytes);
                    }
                    else
                    {
                        return this.Content(jsonObject["document"] + " is not found");
                    }
                }
                else
                {
                    byte[] bytes = Convert.FromBase64String(jsonObject["document"]);
                    stream = new MemoryStream(bytes);
                }
            }

            jsonResult = pdfviewer.Load(stream, jsonObject);
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
            PdfRenderer pdfviewer = new PdfRenderer(Cache);
            var jsonResult = pdfviewer.GetBookmarks(jsonObject);
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
            PdfRenderer pdfviewer = new PdfRenderer(Cache);
            object jsonResult = pdfviewer.GetPage(jsonObject);
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
            PdfRenderer pdfviewer = new PdfRenderer(Cache);
            object result = pdfviewer.GetThumbnailImages(jsonObject);
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
            PdfRenderer pdfviewer = new PdfRenderer(Cache);
            object jsonResult = pdfviewer.GetAnnotationComments(jsonObject);
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
            PdfRenderer pdfviewer = new PdfRenderer(Cache);
            string? jsonResult = pdfviewer.GetAnnotationComments(jsonObject).SerializeToJson();
            return Content(jsonResult);
        }

        //[AcceptVerbs("Post")]
        //[HttpPost]
        ////Post action to import annotations
        //public IActionResult ImportAnnotations([FromBody] Dictionary<string, string> jsonObject)
        //{
        //    PdfRenderer pdfviewer = new PdfRenderer(Cache);
        //    string jsonResult = string.Empty;
        //    if (jsonObject != null && jsonObject.ContainsKey("fileName"))
        //    {
        //        string documentPath = GetDocumentPath(jsonObject["fileName"]);
        //        if (!string.IsNullOrEmpty(documentPath))
        //        {
        //            jsonResult = System.IO.File.ReadAllText(documentPath);
        //        }
        //        else
        //        {
        //            return this.Content(jsonObject["document"] + " is not found");
        //        }
        //    }
        //    return Content(jsonResult);
        //}

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
            PdfRenderer pdfviewer = new PdfRenderer(Cache);
            pdfviewer.ClearCache(jsonObject);
            return this.Content("Document cache is cleared");
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
            PdfRenderer pdfviewer = new PdfRenderer(Cache);
            string documentBase = pdfviewer.GetDocumentAsBase64(jsonObject);
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
            PdfRenderer pdfviewer = new PdfRenderer(Cache);
            object pageImage = pdfviewer.GetPrintImage(jsonObject);
            return Content(JsonConvert.SerializeObject(pageImage));
        }

        //Gets the path of the PDF document
        //private string GetDocumentPath(string document)
        //{
        //    string documentPath = string.Empty;
        //    if (!System.IO.File.Exists(document))
        //    {
        //        var path = _hostingEnvironment.ContentRootPath;
        //        if (System.IO.File.Exists(path + "\\Data\\" + document))
        //            documentPath = path + "\\Data\\" + document;
        //    }
        //    else
        //    {
        //        documentPath = document;
        //    }
        //    return documentPath;
        //}
    }
}