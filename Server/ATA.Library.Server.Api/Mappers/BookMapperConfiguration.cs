using ATA.Library.Server.Model.Book;
using ATA.Library.Server.Model.Entities.Book;
using ATA.Library.Server.Model.Enums;
using AutoMapper;
using System.Linq;

namespace ATA.Library.Server.Api.Mappers
{
    public class BookMapperConfiguration : Profile
    {
        public BookMapperConfiguration()
        {
            CreateMap<BookEntity, BookDto>()
                .ReverseMap()

                .ForMember(entity => entity.CoverImageFileFormat, opts =>
                    opts.MapFrom(dto =>
                        dto.FileData.Where(f => f.FileType == FileType.CoverImage).Select(f => f.MimeType)))

                .ForMember(entity => entity.BookFileFormat, opts =>
                    opts.MapFrom(dto =>
                        dto.FileData.Where(f => f.FileType == FileType.BookPdf).Select(f => f.MimeType)))

                .ForMember(entity => entity.BookFileSize, opts =>
                    opts.MapFrom(dto =>
                        dto.FileData.Where(f => f.FileType == FileType.BookPdf).Select(f => f.Size)));
        }
    }
}