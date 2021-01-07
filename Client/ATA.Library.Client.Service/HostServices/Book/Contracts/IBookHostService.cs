using ATA.Library.Shared.Dto;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ATA.Library.Client.Service.HostServices.Book.Contracts
{
    public interface IBookHostService
    {
        Task<ApiResult<BookDto>?> GetBookById(int bookId);

        Task<ApiResult<List<BookDto>>?> GetBooksByCategory(int categoryId);

        Task<ApiResult<string>?> PostUploadBookFile(MultipartFormDataContent fileContent);

        Task<ApiResult?> PostAddBook(BookDto book);

        Task<ApiResult?> PutEditBook(BookDto book);

        Task<ApiResult?> DeleteBook(int bookId);
    }
}