using ATA.Library.Server.Model.Book;
using ATA.Library.Shared.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATA.Library.Client.Service.HostServices.Book.Contracts
{
    public interface IBookHostService
    {
        Task<ApiResult<BookDto>?> GetBookById(int bookId);

        Task<ApiResult<List<BookDto>>?> GetBooksByCategory(int categoryId);

        Task<ApiResult?> AddBook(BookDto book);

        Task<ApiResult?> EditBook(BookDto book);

        Task<ApiResult?> DeleteBook(int bookId);
    }
}