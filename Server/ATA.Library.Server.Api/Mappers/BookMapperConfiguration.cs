using ATA.Library.Server.Model.Book;
using ATA.Library.Server.Model.Entities.Book;
using AutoMapper;

namespace ATA.Library.Server.Api.Mappers
{
    public class BookMapperConfiguration : Profile
    {
        public BookMapperConfiguration()
        {
            CreateMap<BookDto, BookEntity>()
                .ReverseMap();
        }
    }
}