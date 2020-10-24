using EvanBackstageApi.Entity.CEG;
using EvanBackstageApi.Entity.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EvanBackstageApi.IService.ICEGService
{
    public  interface IEmployeesService : IBaseService<Employee>
    {
        LoginUserInfo GetLoginInfo(string accessToken);
        void CreateTable(bool Backup = false, int StringDefaultLength = 100, params Type[] types);
        Guid GetEcompanyInfo(Expression<Func<Employee, bool>> expression, Expression<Func<Employee, Guid>> expression2);
    }
}
