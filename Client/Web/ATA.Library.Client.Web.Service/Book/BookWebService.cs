using ATA.Library.Client.Service.HostServices.Book.Contracts;
using ATA.Library.Client.Web.Service.Book.Contracts;
using ATA.Library.Shared.Dto;
using Blazored.Toast.Services;
using System.Collections.Generic;
using System.Net.Http;
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
#if DEBUG
                throw new HttpRequestException(msg);
#endif
            }

            if (!getBookResult.IsSuccess)
            {
                _toastService.ShowError(getBookResult.Message);
#if DEBUG
                throw new HttpRequestException(getBookResult.Message ?? "HttpGet call failed");
#endif
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
#if DEBUG
                throw new HttpRequestException(msg);
#endif
            }

            if (!getBooksResult.IsSuccess)
            {
                _toastService.ShowError(getBooksResult.Message);
#if DEBUG
                throw new HttpRequestException(getBooksResult.Message ?? "HttpGet call failed");
#endif
            }

            return getBooksResult.Data!;
        }

        public async Task AddBook(BookDto book)
        {
            var addResult = await _bookHostService.PostAddBook(book);

            if (addResult == null)
            {
                var msg = "ارتباط با سرور برقرار نشد. لطفا چند دقیقه بعد مجدد تلاش نمایید";
                _toastService.ShowError(msg);
#if DEBUG
                throw new HttpRequestException(msg);
#endif

            }

            if (!addResult.IsSuccess)
            {
                _toastService.ShowError(addResult.Message);
#if DEBUG
                throw new HttpRequestException(addResult.Message ?? "HttpPost call failed");
#endif

            }
        }

        public async Task<string> UploadBookFile(MultipartFormDataContent fileContent)
        {
            var uploadResult = await _bookHostService.PostUploadBookFile(fileContent);

            if (uploadResult == null)
            {
                var msg = "ارتباط با سرور برقرار نشد. لطفا چند دقیقه بعد مجدد تلاش نمایید";
                _toastService.ShowError(msg);
#if DEBUG
                throw new HttpRequestException(msg);
#endif

            }

            if (!uploadResult.IsSuccess)
            {
                _toastService.ShowError(uploadResult.Message);
#if DEBUG
                throw new HttpRequestException(uploadResult.Message ?? "HttpPost call failed");
#endif
            }

            return uploadResult.Data!;
        }

        public async Task EditBook(BookDto book)
        {
            var editResult = await _bookHostService.PutEditBook(book);

            if (editResult == null)
            {
                var msg = "خطا در ارتباط با سرور";
                _toastService.ShowError(msg);
#if DEBUG
                throw new HttpRequestException(msg);
#endif

            }

            if (!editResult.IsSuccess)
            {
                _toastService.ShowError(editResult.Message);
#if DEBUG
                throw new HttpRequestException(editResult.Message ?? "HttpPut call failed");
#endif
            }
        }

        public async Task DeleteBook(int bookId)
        {
            var deleteResult = await _bookHostService.DeleteBook(bookId);

            if (deleteResult == null)
            {
                var msg = "خطا در ارتباط با سرور";
                _toastService.ShowError(msg);
#if DEBUG
                throw new HttpRequestException(msg);
#endif
            }

            if (!deleteResult.IsSuccess)
            {
                _toastService.ShowError(deleteResult.Message);
#if DEBUG
                throw new HttpRequestException(deleteResult.Message ?? "HttpDelete call failed");
#endif
            }
        }
    }
}