using EvanBackstageApi.Entity.UserInfo;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvanBackstageApi.IRepository
{
    public interface IUnitOfWork
    {
        ISqlSugarClient GetDbClient();

        void BeginTran();

        void CommitTran();
        void RollbackTran();
        LoginUserInfo GetInfo(string accessToken);
    }
}
