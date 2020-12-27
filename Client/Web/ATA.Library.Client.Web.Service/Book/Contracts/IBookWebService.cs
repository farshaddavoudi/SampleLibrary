using ATA.Library.Server.Model.Book;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.Service.Book.Contracts
{
    public interface IBookWebService
    {
        Task<BookDto> GetBookById(int bookId);

        Task<List<BookDto>> GetBooksByCategory(int categoryId);

        Task AddBook(BookDto book);

        Task EditBook(BookDto book);

        Task DeleteBook(int bookId);

    }
}