using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAuthorizeInfo.Connected
{
    public interface IUnitOfWork
    {
        ISqlSugarClient GetDbClient();

        void BeginTran();

        void CommitTran();
        void RollbackTran();
        void CreateTable(bool Backup = false, int StringDefaultLength = 1000, params Type[] types);
    }
}
