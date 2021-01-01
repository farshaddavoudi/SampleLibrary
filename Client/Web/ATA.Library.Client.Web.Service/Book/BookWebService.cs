using ATA.Library.Client.Service.HostServices.Book.Contracts;
using ATA.Library.Client.Web.Service.Book.Contracts;
using ATA.Library.Shared.Dto;
using ATA.Library.Shared.Service.Exceptions;
using Blazored.Toast.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.Service.Book
{
    public class BookWebService : IBookWebService
    {
        private readonly IBookHostService _bookHostService;
        private readonly IToastService _toastService;

        #region Constructor Injections

        public BookWebService(IBookHostService bookHostService, IToastService toastService)
        {
            _bookHostService = bookHostService;
            _toastService = toastService;
        }

        #endregion

        public async Task<BookDto> GetBookById(int bookId)
        {
            var getBookResult = await _bookHostService.GetBookById(bookId);

            if (getBookResult == null)
            {
                var msg = "ارتباط با سرور برقرار نشد. لطفا چند دقیقه بعد مجدد تلاش نمایید";
                _toastService.ShowError(msg);
                throw new BadRequestException(msg);
            }

            if (!getBookResult.IsSuccess)
            {
                _toastService.ShowError(getBookResult.Message);
                throw new BadRequestException(getBookResult.Message ?? "HttpGet call failed");
            }

            return getBookResult.Data!;
        }

        public async Task<List<BookDto>> GetBooksByCategory(int categoryId)
        {
            var getBooksResult = await _bookHostService.GetBooksByCategory(categoryId);

            if (getBooksResult == null)
            {
                var msg = "ارتباط با سرور برقرار نشد. لطفا چند دقیقه بعد مجدد تلاش نمایید";
                _toastService.ShowError(msg);
                throw new BadRequestException(msg);
            }

            if (!getBooksResult.IsSuccess)
            {
                _toastService.ShowError(getBooksResult.Message);
                throw new BadRequestException(getBooksResult.Message ?? "HttpGet call failed");
            }

            return getBooksResult.Data!;
        }

        public async Task AddBook(BookDto book)
        {
            var addResult = await _bookHostService.AddBook(book);

            if (addResult == null)
            {
                var msg = "ارتباط با سرور برقرار نشد. لطفا چند دقیقه بعد مجدد تلاش نمایید";
                _toastService.ShowError(msg);
                throw new DomainLogicException(msg);
            }

            if (!addResult.IsSuccess)
            {
                _toastService.ShowError(addResult.Message);
                throw new DomainLogicException(addResult.Message ?? "HttpPost call failed");
            }
        }

        public async Task EditBook(BookDto book)
        {
            var editResult = await _bookHostService.EditBook(book);

            if (editResult == null)
            {
                var msg = "خطا در ارتباط با سرور";
                _toastService.ShowError(msg);
                throw new DomainLogicException(msg);
            }

            if (!editResult.IsSuccess)
            {
                _toastService.ShowError(editResult.Message);
                throw new DomainLogicException(editResult.Message ?? "HttpPut call failed");
            }
        }

        public async Task DeleteBook(int bookId)
        {
            var deleteResult = await _bookHostService.DeleteBook(bookId);

            if (deleteResult == null)
            {
                var msg = "خطا در ارتباط با سرور";
                _toastService.ShowError(msg);
                throw new DomainLogicException(msg);
            }

            if (!deleteResult.IsSuccess)
            {
                _toastService.ShowError(deleteResult.Message);
                throw new DomainLogicException(deleteResult.Message ?? "HttpDelete call failed");
            }
        }
    }
}