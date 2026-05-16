using AutoMapper;
using Personal_Blogging_Platform.Data.DTOs.auth;
using Personal_Blogging_Platform.Data.Entities;

namespace Personal_Blogging_Platform.Data.Mapping
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, User>().ForMember(
                dest => dest.HashedPassword,
                opt => opt.Ignore()).ReverseMap();
                
        }
    }
}
