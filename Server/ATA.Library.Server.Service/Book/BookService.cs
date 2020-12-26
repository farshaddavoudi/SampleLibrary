using ATA.Library.Server.Model.Book;
using ATA.Library.Server.Model.Entities.Book;
using ATA.Library.Server.Model.Enums;
using ATA.Library.Server.Service.Book.Contracts;
using ATA.Library.Server.Service.Contracts;
using ATA.Library.Shared.Service.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using MimeTypes;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ATA.Library.Server.Service.Book
{
    public class BookService : EntityService<BookEntity>, IBookService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public BookService(IUserInfoProvider userInfoProvider, IATARepository<BookEntity> repository, IMapper mapper, IHostingEnvironment hostingEnvironment) : base(userInfoProvider, repository, mapper)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> SaveFileAndGetPathAsync(FileData file, CancellationToken cancellationToken)
        {
            var filePath = FilePath(file);

            var path = Path.Combine(_hostingEnvironment.ContentRootPath, filePath);

            await using var fileStream = File.Create(path);

            await fileStream.WriteAsync(file.Data, cancellationToken);

            return filePath;
        }

        private string FilePath(FileData file)
        {
            string fileExtenstion = MimeTypeMap.GetExtension(file.MimeType);

            string fileName = $"{Guid.NewGuid()}.{fileExtenstion}";

            if (file.FileType == FileType.CoverImage)
                return $"Uploads/CoverImages/{fileName}";

            if (file.FileType == FileType.BookPdf)
                return $"Uploads/BookFiles/{fileName}";

            throw new BadRequestException();
        }
    }
}