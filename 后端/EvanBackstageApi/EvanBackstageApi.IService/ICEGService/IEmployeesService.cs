using EvanBackstageApi.Entity.CEG;
using EvanBackstageApi.Entity.UserInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvanBackstageApi.IService.ICEGService
{
    public  interface IEmployeesService : IBaseService<Employee>
    {
        LoginUserInfo GetLoginInfo(string accessToken);
    }
}
