using AutoMapper;
using Personal_Blogging_Platform.Data.DTOs.Category;
using Personal_Blogging_Platform.Data.Entities;

namespace Personal_Blogging_Platform.Data.Mapping
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category,CategoryResponseDto>();
            CreateMap<CategoryRequestDto,Category>();
        }
    }
}
