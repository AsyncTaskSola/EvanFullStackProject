
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Root;
using EvanBackstageApi.Entity.UserInfo;
using EvanBackstageApi.IRepository;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EvanBackstageApi.Repository
{
    /// <summary>
    /// 数据库连接
    /// </summary>
    public class UnitOfWork: IUnitOfWork
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
        public LoginUserInfo GetInfo(string accessToken = null)
        {

            loginUserInfo = _sqlSugarClient.Queryable<LoginUserInfo>().Where(x => x.AccessToken == accessToken).First();
            var resultNew = _sqlSugarClient.Queryable<LoginUserInfo>().Where(x => x.AccessToken == accessToken).OrderBy(x => x.DateTimeStart, OrderByType.Desc).First();
            loginUserInfo.CurrentTime = resultNew.DateTimeStart;
            return loginUserInfo;
        }
        //code first 创建表字段(但需要自定义参数约束，有需求可以用)  现用在项目JwtAuthorizeInfoApi中
        //http://www.codeisbug.com/home/doc?masterId=1&typeId=1203
        public void CreateTable(bool Backup = false, int StringDefaultLength = 100, params Type[] entitys)
        {
            _sqlSugarClient.CodeFirst.SetStringDefaultLength(StringDefaultLength);
            Console.WriteLine("Init Tables...");
            var result = false;
            entitys.ToList().ForEach(x =>
            {
                if (!_sqlSugarClient.DbMaintenance.IsAnyTable(x.Name))
                {
                    //数据表不存在直接默认创建
                    Console.WriteLine($"Create Table:{x.Name}");
                    if (Backup)
                    {
                        _sqlSugarClient.CodeFirst.BackupTable().InitTables(entitys);
                    }
                    else
                    {
                        _sqlSugarClient.CodeFirst.InitTables(entitys);
                    }
                    result = true;    
                }
                else
                {
                    //当数据表存在，列名不存在
                    var fields = x.GetProperties().ToList();
                    //获取当前表所有列名的集合
                    var columns = _sqlSugarClient.DbMaintenance.GetColumnInfosByTableName(x.Name).ToList();
                    var needUpdate = Check(fields, columns);   
                    Console.WriteLine($"Update Table:{x.Name}");
                    if (Backup)
                    {
                        _sqlSugarClient.CodeFirst.BackupTable().InitTables(x);
                    }
                    else
                    {
                        _sqlSugarClient.CodeFirst.InitTables(x);
                    }
                    result = true;
                }
            });
            if (result)
            {
                Console.WriteLine("Init Tables Finish");
            }
            else
            {
                Console.WriteLine("No Tables Need To Init");
            }
        }
        //public SimpleClient<Employee> EmployeeDb { get { return new SimpleClient<Employee>(_sqlSugarClient); } }
        /// <summary>
        /// 检查数据表是否需要更新
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        private static bool Check(List<PropertyInfo> fields, List<DbColumnInfo> columns)
        {
            var fieldNames = fields.Select(f => f.Name);
            var columnNames = columns.Select(c => c.DbColumnName);
            if (fields.Count == columns.Count)
            {
               var rs = Enumerable.Except(fieldNames, columnNames).ToList();
                if (rs.Count != 0)
                {
                    return true;
                }
                return false;
            }
            return true;
        }
    

        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <param name="sugar"></param>
        /// <returns></returns>
        public async Task Init()
        {
            await Task.Run(() =>
            {
                // 创建数据库
                _sqlSugarClient.DbMaintenance.CreateDatabase();
                // 获取当前项目所有程序集  坑到爆炸
                //var models = Assembly.GetExecutingAssembly().GetTypes();
                //获取指定路径项目所有程序集
                var models = Assembly.Load("EvanBackstageApi.Entity").GetTypes();
                // 获取命名空间为xx的程序集
                var entitys = models.Where(m => m.Namespace == "EvanBackstageApi.Entity.JwtAuthorizeInfo.Root").ToArray();
                CreateTable(false, 1000, entitys);
                // 初始化数据
                CreateSeed(_sqlSugarClient);
            });
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="db"></param>
        private static void CreateSeed(ISqlSugarClient db)
        {
            #region 用户表初始
            if (!db.Queryable<User>().Any(u => u.Account == "Evan"))
            {
                try
                {
                    Console.WriteLine("创建初始用户");
                    var user = new User()
                    {
                        Id = new Guid("e4c7bd88-e593-4ef3-ab9b-0a4d74b76ec5"),
                        Name = "Evan",
                        Account = "Evan",
                        Password = MD5Helper.Md5Str("123456"),
                        IsInit = true
                    };
                    db.Insertable(user).ExecuteCommand();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            #endregion

            #region 角色表初始
            if(!db.Queryable<Role>().Any(x=>x.Code=="Admin"))
            {
                try
                {
                    Console.WriteLine("创建Admin");
                    var roles = new List<Role>
                    {
                        new Role()
                        {
                            Name="管理员",
                            Code="Admin",
                            Describe="最高权限",
                            IsInit=true
                        },
                        new Role()
                        {
                            Name = "开发人员",
                            Code = "Developement",
                            Describe = "开发",
                            IsInit = true
                        }
                    };
                    db.Insertable(roles).ExecuteCommand();
                    Console.WriteLine("初始用户关联Admin权限");
                    var user = db.Queryable<User>().First(u => u.Name == "Evan");
                    var Role = db.Queryable<Role>().First(r => r.Code == "Admin");
                    var userrole = new UserRole
                    {
                        UserId = user.Id,
                        RoleId= Role.Id
                    };
                    db.Insertable(userrole).ExecuteCommand();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            #endregion

            #region 菜单表初始
            //if (!db.Queryable<RoleMenu>().Any())
            //{
            //    try
            //    {
            //        Console.WriteLine("初始菜单表");
            //        Console.WriteLine("初始一级菜单");
            //        var menus = new List<Menu>
            //        {
            //            new Menu()
            //            {
            //                Name = "首页",
            //                Icon = "el-icon-s-home",
            //                Module = "home",
            //                IsInit = true,
            //                Index = 1
            //            },
            //            new Menu()
            //            {
            //                Name = "工单项目",
            //                Icon = "el-icon-s-platform",
            //                Module = "projects",
            //                IsInit = true,
            //                Index = 2
            //            },
            //            new Menu()
            //            {
            //                Name = "用户管理",
            //                Icon = "el-icon-user-solid",
            //                Module = "users",
            //                IsInit = true,
            //                IsAuth = true,
            //                Index = 97
            //            },
            //            new Menu()
            //            {
            //                Name = "权限管理",
            //                Icon = "el-icon-s-help",
            //                Module = "auth",
            //                IsInit = true,
            //                IsAuth = true,
            //                Index = 98
            //            },
            //            new Menu()
            //            {
            //                Name = "系统设置",
            //                Icon = "el-icon-s-tools",
            //                Module = "system",
            //                IsInit = true,
            //                Index = 99
            //            }
            //        };
            //        db.Insertable<Menu>(menus).ExecuteCommand();
            //        Console.WriteLine("初始权限管理二级菜单");
            //        var auth = db.Queryable<Menu>().First(m => m.Name == "权限管理");
            //        var authMenus = new List<Menu>
            //        {
            //            new Menu()
            //            {
            //                Name = "角色管理",
            //                Module = "roles",
            //                Pid = auth.Id,
            //                IsInit = true,
            //                IsAuth = true,
            //                Index = 1
            //            },
            //            new Menu()
            //            {
            //                Name = "菜单设置",
            //                Module = "menus",
            //                Pid = auth.Id,
            //                IsInit = true,
            //                IsAuth = true,
            //                Index = 2
            //            },
            //        };
            //        db.Insertable<Menu>(authMenus).ExecuteCommand();
            //        Console.WriteLine("权限菜单关联Admin");
            //        // 需要关联的菜单
            //        var needMenus = db.Queryable<Menu>().Where(m => m.Name == "用户管理" || m.Name == "权限管理" || m.Name == "角色管理" || m.Name == "菜单设置").ToList();

            //        var admin = db.Queryable<Role>().First(r => r.Code == "Admin");
            //        var roleMenus = new List<RoleMenu>();
            //        needMenus.ForEach(m =>
            //        {
            //            var rm = new RoleMenu
            //            {
            //                RoleId = admin.Id,
            //                MenuId = m.Id
            //            };
            //            roleMenus.Add(rm);
            //        });
            //        db.Insertable<RoleMenu>(roleMenus).ExecuteCommand();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception(ex.Message);
            //    }
            //}
            #endregion
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
