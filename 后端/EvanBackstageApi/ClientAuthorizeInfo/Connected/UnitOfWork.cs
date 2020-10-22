using EvanBackstageApi.Entity.UserInfo;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAuthorizeInfo.Connected
{
    /// <summary>
    /// 数据库连接
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IConfiguration _configuration;
        private readonly ISqlSugarClient _sqlSugarClient;
        public string result;
        public UnitOfWork(IConfiguration Configuration)
        {
            _configuration = Configuration;
            result = _configuration["SqlConnect:Sql"];
            _sqlSugarClient = new SqlSugarClient(
                new ConnectionConfig()
                {
                    ConnectionString = _configuration["SqlConnect:Sql"],
                    DbType = DbType.SqlServer,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute  // Attribute用于DbFirst  从数据库生成model的
                }
            );
            _sqlSugarClient.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" + _sqlSugarClient.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
        }
        //启发 https://www.cnblogs.com/mvpbest/p/13291355.html
        public void CreateTable(bool Backup = false, int StringDefaultLength = 1000, params Type[] types)
        {
            _sqlSugarClient.CodeFirst.SetStringDefaultLength(StringDefaultLength);
            _sqlSugarClient.DbMaintenance.CreateDatabase();
            if (Backup)
            {
                _sqlSugarClient.CodeFirst.BackupTable().InitTables(types);
            }
            else
            {
                _sqlSugarClient.CodeFirst.InitTables(types);
            }
        }
        public SimpleClient<LoginUserInfo> LoginUserInfoDb { get { return new SimpleClient<LoginUserInfo>(_sqlSugarClient); } }
        public ISqlSugarClient GetDbClient()
        {

            return _sqlSugarClient;
        }
        public void BeginTran()
        {
            GetDbClient().Ado.BeginTran();
        }

        public void CommitTran()
        {
            try
            {
                GetDbClient().Ado.CommitTran();
            }
            catch (Exception ex)
            {
                GetDbClient().Ado.RollbackTran();
                throw ex;
            }
        }
        /// <summary>
        /// 出错回滚
        /// </summary>
        public void RollbackTran()
        {
            GetDbClient().Ado.RollbackTran();
        }
    }
}
