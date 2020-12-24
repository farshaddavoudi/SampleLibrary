using ATA.Library.Server.Model.Book;
using ATA.Library.Server.Service.Book.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ATA.Library.Server.Api.Controllers.api.Book
{
    /// <summary>
    /// Book managements
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

        [HttpGet("get-by-category")]
        public async Task<IActionResult> GetBooksByCategory(int categoryId, CancellationToken cancellationToken)
        {
            return null;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddBook(BookDto dto, CancellationToken cancellationToken)
        {
            return null;
        }

        [HttpPut("edit")]
        public async Task<IActionResult> EditBook(int bookId, BookDto dto, CancellationToken cancellationToken)
        {
            return null;
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteBook(int bookId, CancellationToken cancellationToken)
        {
            return null;
        }

    }
}