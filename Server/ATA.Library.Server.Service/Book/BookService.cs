using ATA.Library.Server.Model.Book;
using ATA.Library.Server.Model.Entities.Book;
using ATA.Library.Server.Model.Enums;
using ATA.Library.Server.Service.Book.Contracts;
using ATA.Library.Server.Service.Contracts;
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
            string fileExtenstion = MimeTypeMap.GetExtension(file.MimeType);

            string fileRelativeUrl = $"{Guid.NewGuid()}.{fileExtenstion}";

            var path = file.FileType == FileType.CoverImage
                ? Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads", "CoverImages", fileRelativeUrl)
                : Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads", "BookFiles", fileRelativeUrl);

            await using var fileStream = File.Create(path);

            await fileStream.WriteAsync(file.Data, cancellationToken);

            return path;
        }
    }
}