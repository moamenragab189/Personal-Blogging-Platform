using AutoMapper;
using Personal_Blogging_Platform.Data.DTOs.Post;
using Personal_Blogging_Platform.Data.Entities;

namespace Personal_Blogging_Platform.Data.Mapping
{
    public class PostProfile:Profile
    {
        public PostProfile()
        {
            CreateMap<PostRequestDto, Post>();
            CreateMap<Post, PostResponseDto>();
        }
    }
}
