using ATA.Library.Server.Model.Entities.Book;
using ATA.Library.Server.Service.Book.Contracts;
using ATA.Library.Server.Service.Contracts;
using AutoMapper;

namespace ATA.Library.Server.Service.Book
{
    public class BookService : EntityService<BookEntity>, IBookService
    {
        public BookService(IUserInfoProvider userInfoProvider, IATARepository<BookEntity> repository, IMapper mapper) : base(userInfoProvider, repository, mapper)
        {
        }
    }
}