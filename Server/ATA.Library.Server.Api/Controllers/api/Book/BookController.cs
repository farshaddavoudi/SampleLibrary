using ATA.Library.Server.Model.Entities.Book;
using ATA.Library.Server.Service.Book.Contracts;
using ATA.Library.Shared.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ATA.Library.Server.Api.Controllers.api.Book
{
    #region Args

    // Declare a class that stores chunk details.
    public class ChunkMetadata
    {
        public int Index { get; set; }
        public int TotalCount { get; set; }
        public int FileSize { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public string? FileGuid { get; set; }
    }

    #endregion

    /// <summary>
    /// Books management
    /// </summary>
    [ApiVersion("1")]
    public class BookController : BaseApiController
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly ILogger<BookController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        #region Constructor Injections

        public BookController(IBookService bookService, IMapper mapper, ILogger<BookController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _bookService = bookService;
            _mapper = mapper;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        #endregion


        /// <summary>
        /// Get book by its unique identifier
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetBookById(int bookId, CancellationToken cancellationToken)
        {
            var book = await _bookService.GetByIdAsync(bookId, cancellationToken);

            if (book == null)
                return NotFound($"هیچ کتابی با این شناسه پیدا نشد. شناسه‌ی ارسالی = {bookId}");

            return Ok(_mapper.Map<BookDto>(book));
        }

        /// <summary>
        /// Get books by category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("get-by-category")]
        public async Task<IActionResult> GetBooksByCategory(int categoryId, CancellationToken cancellationToken)
        {
            var categoryBooks = await _bookService.GetAll()
                .Include(b => b.Category)
                .Where(b => b.CategoryId == categoryId)
                .ToListAsync(cancellationToken);

            return Ok(_mapper.Map<List<BookDto>>(categoryBooks));
        }


        /// <summary>
        /// Upload book pdf file into server by chunks (using DevExpress component)
        /// </summary>
        /// <param name="bookFile">"bookFile" is the value of the Upload "Name" property.</param>
        /// <param name="chunkMetadata"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("upload-file")]
        public async Task<ActionResult> UploadBookFile(IFormFile bookFile, [FromForm] string chunkMetadata, CancellationToken cancellationToken)
        {
            // Specify the location for temporary files.
            var tempPath = Path.Combine(_hostingEnvironment.ContentRootPath, "TempChunkedUploadFiles");

            // Remove temporary files.
            _bookService.RemoveTempFilesAfterDelay(tempPath, new TimeSpan(0, 5, 0));

            try
            {
                if (!string.IsNullOrEmpty(chunkMetadata))
                {
                    // Get chunk details.
                    var metaDataObject = JsonConvert.DeserializeObject<ChunkMetadata>(chunkMetadata);

                    // Specify the full path for temporary files (including the file name).
                    var tempFilePath = Path.Combine(tempPath, metaDataObject.FileGuid + ".tmp");

                    // Check whether the target directory exists; otherwise, create it.
                    if (!Directory.Exists(tempPath))
                        Directory.CreateDirectory(tempPath);

                    // Append the chunk to the file.
                    await _bookService.AppendChunkToFileAsync(tempFilePath, bookFile, cancellationToken);

                    // Save the file if all chunks are received.
                    if (metaDataObject.Index == (metaDataObject.TotalCount - 1))
                        _bookService.SaveUploadedFile(tempFilePath, metaDataObject.FileName!);
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// Add a book
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
        public async Task<IActionResult> AddBook(BookDto dto, CancellationToken cancellationToken)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            dto.CoverImageUrl = dto.CoverImageByteData != null
                ? await _bookService.SaveCoverImageFileAndGetPathAsync(dto.CoverImageByteData,
                    dto.CoverImageFileFormat!, cancellationToken)
                : "default-book-cover.png";

            var entity = _mapper.Map<BookEntity>(dto);

            var addedEntity = await _bookService.AddAsync(entity, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, addedEntity);
        }

        /// <summary>
        /// Edit an already existing book
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("edit")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> EditBook(int bookId, BookDto dto, CancellationToken cancellationToken)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = await _bookService.GetByIdAsync(bookId, cancellationToken);

            if (entity == null)
                return NotFound($"هیچ کتابی با این شناسه پیدا نشد. شناسه‌ی ارسالی = {bookId}");

            dto.CreatedAt = entity.CreatedAt; //fix CreateAt mapping issue

            dto.CoverImageUrl = dto.CoverImageByteData != null
                ? await _bookService.SaveCoverImageFileAndGetPathAsync(dto.CoverImageByteData,
                    dto.CoverImageFileFormat!, cancellationToken)
                : "default-book-cover.png";

            _mapper.Map(dto, entity);

            await _bookService.UpdateAsync(entity, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Delete a book
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<IActionResult> DeleteBook(int bookId, CancellationToken cancellationToken)
        {
            var entity = await _bookService.GetByIdAsync(bookId, cancellationToken);

            if (entity == null)
                return NotFound($"هیچ کتابی با این شناسه پیدا نشد. شناسه‌ی ارسالی = {bookId}");

            await _bookService.DeleteAsync(bookId, cancellationToken);

            return NoContent();
        }

    }


}