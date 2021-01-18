using AutoMapper;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.JwtViewMapperDtoModel.UserProfiles;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Root;

namespace EvanBackstageApi.Extensions.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, V_UserDto>();
            CreateMap<V_UserLoginDto, User>();
            CreateMap<V_UserUpdateDto, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(s => s.NewPassword));
            CreateMap<User, V_UsersViewDto>();
            CreateMap<V_UserAddDto, User>();
            CreateMap<V_UserUpdateDto, V_UserDto>();
            CreateMap<V_SysUserDto, User>();
            CreateMap<User, V_SysUserDto>();
        }     
    }
}
