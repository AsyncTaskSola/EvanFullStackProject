using EvanBackstageApi.Entity.UserInfo;
using EvanBackstageApi.IRepository;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvanBackstageApi.Repository
{
    /// <summary>
    /// 数据库连接
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IConfiguration _configuration;
        private readonly ISqlSugarClient _sqlSugarClient;
        public string result;
        public LoginUserInfo loginUserInfo;
        public UnitOfWork(IConfiguration Configuration)
        {
            _configuration = Configuration;
            result = _configuration["SqlConnect:Sql"];
            _sqlSugarClient = new SqlSugarClient(
                new ConnectionConfig()
                {
                    ConnectionString = _configuration["SqlConnect:Sql"],
                    DbType = DbType.SqlServer,
                    IsAutoCloseConnection = true
                }
            );
            _sqlSugarClient.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" + _sqlSugarClient.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
        }
        public LoginUserInfo GetInfo(string accessToken = null)
        {
            loginUserInfo = _sqlSugarClient.Queryable<LoginUserInfo>().Where(x => x.AccessToken == accessToken).First();
            var resultNew = _sqlSugarClient.Queryable<LoginUserInfo>().Where(x => x.AccessToken == accessToken).OrderBy(x => x.DateTimeStart, OrderByType.Desc).First();
            loginUserInfo.CurrentTime = resultNew.DateTimeStart;
            return loginUserInfo;
        }
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
