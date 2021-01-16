using ATA.Library.Server.Model.AppSettings;
using ATA.Library.Server.Model.Entities.Book;
using ATA.Library.Server.Service.Book.Contracts;
using ATA.Library.Server.Service.Contracts;
using ATA.Library.Shared.Service.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ATA.Library.Server.Service.Book
{
    public class BookService : EntityService<BookEntity>, IBookService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly AppSettings _appSettings;

        #region Constructor Injections
        public BookService(IUserInfoProvider userInfoProvider, IATARepository<BookEntity> repository, IMapper mapper, IHostingEnvironment hostingEnvironment, AppSettings appSettings) : base(userInfoProvider, repository, mapper)
        {
            _hostingEnvironment = hostingEnvironment;
            _appSettings = appSettings;
        }
        #endregion

        public async Task<string> SaveCoverImageFileAndGetPathAsync(byte[] fileData, string fileExtension, CancellationToken cancellationToken)
        {
            string fileName = $"{Guid.NewGuid()}{fileExtension}";

            var fullPath = $@"{_appSettings.FileUploadPath!.CoverImage}\{fileName}";

            await using var fileStream = File.Create(fullPath);

            await fileStream.WriteAsync(fileData, cancellationToken);

            return fileName;
        }

        [Obsolete("Replaced by SaveUploadedFile method for DevExpress uploader")]
        public async Task<string> SaveBookFileAndGetPathAsync(byte[] fileData, string bookName, CancellationToken cancellationToken)
        {
            string fileName = $"{bookName}-{Guid.NewGuid()}.pdf";

            var fullPath = $@"{_appSettings.FileUploadPath!.BookFile}\{fileName}";

            await using var fileStream = File.Create(fullPath);

            await fileStream.WriteAsync(fileData, cancellationToken);

            return fileName;
        }

        public async Task<string> GetBookFileAbsoluteUrl(int bookId, CancellationToken cancellationToken)
        {
            var book = await GetByIdAsync(bookId, cancellationToken);

            if (book == null)
                throw new BadRequestException("کتابی با این مشخصات پیدا نشد");

            return $"{_appSettings.FileUploadPath!.BookFile}/{book.BookFileUrl}";
        }

        public void RemoveTempFilesAfterDelay(string path, TimeSpan delay)
        {
            var dir = new DirectoryInfo(path);
            if (dir.Exists)
                foreach (var file in dir.GetFiles("*.tmp").Where(f => f.LastWriteTimeUtc.Add(delay) < DateTime.UtcNow))
                    file.Delete();
        }

        public async Task AppendChunkToFileAsync(string path, IFormFile content, CancellationToken cancellationToken)
        {
            await using var stream = new FileStream(path, FileMode.Append, FileAccess.Write);
            await content.CopyToAsync(stream, cancellationToken);
        }

        public void SaveUploadedFile(string tempFilePath, string fileName)
        {
            var path = $@"{_appSettings.FileUploadPath!.BookFile}";
            File.Copy(tempFilePath, Path.Combine(path, fileName));
        }
    }
}