using EvanBackstageApi.Entity.JwtAuthorizeInfo.Base;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Root;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EvanBackstageApi.IService.IJwtAuthorizeInfoService
{
    public  interface IRoleService : IBaseService<Role>
    {
        Task<JwtResultModel<dynamic>> UpdateUserRoleInfo(UserRole userRole);
    }
}
