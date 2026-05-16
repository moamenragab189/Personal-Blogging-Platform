using AutoMapper;
using Personal_Blogging_Platform.Data.DTOs.Comment;
using Personal_Blogging_Platform.Data.Entities;

namespace Personal_Blogging_Platform.Data.Mapping
{
    public class CommentProfile: Profile
    {
        public CommentProfile()
        {
            CreateMap<CommentDto, Comment>().ReverseMap();
        }
    }
}
