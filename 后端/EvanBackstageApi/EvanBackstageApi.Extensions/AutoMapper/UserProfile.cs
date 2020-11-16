using AutoMapper;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.JwtViewMapperDtoModel.UserProfiles;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Root;

namespace EvanBackstageApi.Extensions.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<V_UserLoginDto, User>();
            CreateMap<User, V_UserDto>();          
        }     
    }
}
