using ATA.Library.Server.Model.Book;
using ATA.Library.Server.Model.Entities.Book;
using ATA.Library.Server.Service.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace ATA.Library.Server.Service.Book.Contracts
{
    public interface IBookService : IEntityService<BookEntity>
    {
        Task<string> SaveFileAndGetPathAsync(FileData fileData, CancellationToken cancellationToken);
    }
}