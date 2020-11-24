using EvanBackstageApi.Entity.UserInfo;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EvanBackstageApi.IRepository
{
    public interface IUnitOfWork
    {
        ISqlSugarClient GetDbClient();

        void BeginTran();

        void CommitTran();
        void RollbackTran();
        LoginUserInfo GetInfo(string accessToken);
        void CreateTable(bool Backup = false, int StringDefaultLength = 100, params Type[] types);
        //Task Init();不可以这样中，没有调用数据库连接部分，导致失败
    }
}
