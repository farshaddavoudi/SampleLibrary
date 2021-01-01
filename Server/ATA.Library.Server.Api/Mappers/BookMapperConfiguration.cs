using ATA.Library.Server.Model.Entities.Book;
using ATA.Library.Shared.Dto;
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