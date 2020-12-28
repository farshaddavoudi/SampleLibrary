using ATA.Library.Server.Model.Book;
using ATA.Library.Server.Model.Entities.Book;
using ATA.Library.Server.Service.Book.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ATA.Library.Server.Api.Controllers.api.Book
{
    /// <summary>
    /// Books management
    /// </summary>
    [ApiVersion("1")]
    [AllowAnonymous]
    public class BookController : BaseApiController
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        #region Constructor Injections

        public BookController(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
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

            return Ok(book);
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
            // todo: Check user has access to this category

            var categoryBooks = await _bookService.GetAll().Where(b => b.CategoryId == categoryId)
                .ToListAsync(cancellationToken);

            return Ok(_mapper.Map<List<BookDto>>(categoryBooks));
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

            // todo: Check user has access to add a book in category

            dto.CoverImageUrl = dto.CoverImageByteData != null
                ? await _bookService.SaveCoverImageFileAndGetPathAsync(dto.CoverImageByteData,
                    dto.CoverImageFileFormat!, cancellationToken)
                : "default-book-cover.png";

            dto.BookFileUrl =
                await _bookService.SaveBookFileAndGetPathAsync(dto.BookFileByteData!, dto.BookFileFormat!,
                    cancellationToken);

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

            // todo: Check user has access to edit this book

            dto.CreatedAt = entity.CreatedAt; //fix CreateAt mapping issue

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

            // todo: Check user has access to this book

            await _bookService.DeleteAsync(bookId, cancellationToken);

            return NoContent();
        }

    }
}