using ATA.Library.Server.Model.Entities.Category;
using ATA.Library.Shared.Dto;
using AutoMapper;

namespace ATA.Library.Server.Api.Mappers
{
    public class CategoryMapperConfiguration : Profile
    {
        public CategoryMapperConfiguration()
        {
            CreateMap<CategoryEntity, CategoryDto>()
                .ReverseMap();
        }
    }
}