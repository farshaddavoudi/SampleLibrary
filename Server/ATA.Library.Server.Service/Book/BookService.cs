using ATA.Library.Server.Model.Entities.Book;
using ATA.Library.Server.Service.Book.Contracts;
using ATA.Library.Server.Service.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
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

        public async Task<string> SaveCoverImageFileAndGetPathAsync(byte[] fileData, string fileExtension, CancellationToken cancellationToken)
        {
            string fileName = $"{Guid.NewGuid()}{fileExtension}";

            var filePath = Path.Combine("Uploads", "CoverImages", fileName);

            var fullPath = Path.Combine(_hostingEnvironment.ContentRootPath, filePath);

            await using var fileStream = File.Create(fullPath);

            await fileStream.WriteAsync(fileData, cancellationToken);

            return filePath;
        }

        public async Task<string> SaveBookFileAndGetPathAsync(byte[] fileData, string fileExtension, CancellationToken cancellationToken)
        {
            string fileName = $"{Guid.NewGuid()}{fileExtension}";

            var filePath = Path.Combine("Uploads", "BookFiles", fileName);

            var fullPath = Path.Combine(_hostingEnvironment.ContentRootPath, filePath);

            await using var fileStream = File.Create(fullPath);

            await fileStream.WriteAsync(fileData, cancellationToken);

            return filePath;
        }

    }
}