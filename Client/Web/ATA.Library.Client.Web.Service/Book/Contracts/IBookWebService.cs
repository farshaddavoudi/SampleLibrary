using ATA.Library.Shared.Dto;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.Service.Book.Contracts
{
    public interface IBookWebService
    {
        Task<BookDto> GetBookById(int bookId);

        Task<List<BookDto>> GetBooksByCategory(int categoryId);

        Task AddBook(BookDto book);

        Task<string> UploadBookFile(MultipartFormDataContent fileContent);

        Task EditBook(BookDto book);

        Task DeleteBook(int bookId);

    }
}