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
        Task<JwtResultModel<V_UserDto>> Login(V_UserLoginDto User);
    }
    
}
