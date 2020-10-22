using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvanBackstageApi.Basic
{
    public class BaseDB
    {
        #region 基本写法

        //public class BaseDB
        //{
        //    private readonly IConfiguration _configuration;

        //    public BaseDB(IConfiguration Configuration)
        //    {
        //        _configuration = Configuration;
        //    }
        //    public SqlSugarClient GetClient()
        //    {
        //        SqlSugarClient db = new SqlSugarClient(
        //            new ConnectionConfig()
        //            {
        //                ConnectionString = _configuration["SqlConnect:Sql"],
        //                DbType = DbType.SqlServer,
        //                IsAutoCloseConnection = true
        //            }
        //        );
        //        db.Aop.OnLogExecuting = (sql, pars) =>
        //        {
        //            Console.WriteLine(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
        //            Console.WriteLine();
        //        };
        //        return db;
        //    }
        //}

        #endregion
    }
}
