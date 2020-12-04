using EvanBackstageApi.Entity.JwtAuthorizeInfo.Base;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.JwtViewMapperDtoModel.UserProfiles;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Root;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EvanBackstageApi.IService.IJwtAuthorizeInfoService
{
    public  interface IUserService : IBaseService<User>
    {
        Task<JwtResultModel<V_UserDto>> Login(V_UserLoginDto UserLoginDto);
        Task<JwtResultModel<dynamic>> Logout(Guid id);
        Task<JwtResultModel<V_UserDto>> Update(V_UserUpdateDto UserUpdateDto);
        Task<JwtResultModel<dynamic>> DeleteId(Guid id);

        Task<JwtResultModel<V_UserDto>> AddUser(V_UserAddDto V_user);

        Task<JwtResultModel<V_SysUserDto>> CheckUserInfo(Guid id);

        Task<JwtResultModel<List<V_UserDto>>> Mapperdata(List<User> userList);
        Task<JwtResultModel<dynamic>> DisableUser(Guid userid);
        Task<List<Role>> GetRoles(Guid userid);
    }
    
}
