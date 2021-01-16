using ATA.Library.Server.Model.Entities.Book;
using ATA.Library.Server.Service.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ATA.Library.Server.Service.Book.Contracts
{
    public interface IBookService : IEntityService<BookEntity>
    {
        Task<string> SaveCoverImageFileAndGetPathAsync(byte[] fileData, string fileExtension,
            CancellationToken cancellationToken);

        Task<string> SaveBookFileAndGetPathAsync(byte[] fileData, string bookName,
            CancellationToken cancellationToken);

        Task<string> GetBookFileAbsoluteUrl(int bookId, CancellationToken cancellationToken);

        void RemoveTempFilesAfterDelay(string path, TimeSpan delay);

        Task AppendChunkToFileAsync(string path, IFormFile content, CancellationToken cancellationToken);

        void SaveUploadedFile(string tempFilePath, string fileName);
    }
}