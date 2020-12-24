using ATA.Library.Server.Model.Entities.Category;
using ATA.Library.Server.Service.Category.Contracts;
using ATA.Library.Server.Service.Contracts;
using AutoMapper;

namespace ATA.Library.Server.Service.Category
{
    public class CategoryService : EntityService<CategoryEntity>, ICategoryService
    {
        public CategoryService(IUserInfoProvider userInfoProvider, IATARepository<CategoryEntity> repository, IMapper mapper) : base(userInfoProvider, repository, mapper)
        {
        }
    }
}