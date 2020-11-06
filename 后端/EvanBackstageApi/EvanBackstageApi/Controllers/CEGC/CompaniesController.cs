using EvanBackstageApi.Basic;
using EvanBackstageApi.Entity.CEG;
using EvanBackstageApi.Entity.UserInfo;
using EvanBackstageApi.Entity.View.V_CEG;
using EvanBackstageApi.IService.ICEGService;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EvanBackstageApi.Controllers.CEGC
{
    [ApiController]
    [Area("CEGC")]
    [Authorize]
    [Route("api/[area]/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private static ILog log = LogManager.GetLogger(Startup.Repository.Name, "CompaniesController");
        private readonly ICompaniesService _iCompaniesService;
        private readonly IEmployeesService _employeesService;
        private readonly IV_CompanyEmployeeInfoServices _iv_CompanyEmployeeInfoServices;
        public static LoginUserInfo UserInfo;

        public CompaniesController(ICompaniesService companiesService,
            IEmployeesService employeesService,
            IV_CompanyEmployeeInfoServices v_CompanyEmployeeInfoServices)
        {
            _iCompaniesService = companiesService;
            _employeesService = employeesService;
            //_employeesService.CreateTable(false, 1000, typeof(Employee));
            _iv_CompanyEmployeeInfoServices = v_CompanyEmployeeInfoServices;
            log.Info("进入到控制器CompaniesController");
        }
        /// <summary>
        /// 添加公司 或者其下的员工
        /// </summary>
        /// <param name="Companies"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<ResultModel<List<Company>>> Add([FromBody] List<Company> Companies)
        {

            try
            {
                Companies.ForEach(async item =>
                {
                    if (item.Name == null) throw new Exception("公司名必填");
                    var data =await _iCompaniesService.QueryFirst(x => x.Name == item.Name);
                    if(data!=null)
                    {
                        throw new Exception("不能添加相同公司名");
                    }
                    var employee = item.Emplyees as List<Employee>;

                    var result =await _iCompaniesService.Add(new Company { Id=item.Id,Introduction=item.Introduction,Name=item.Name,CompanyEmail=item.CompanyEmail,CompanyPhone=item.CompanyPhone,CurrentTime= item.CurrentTime });
                    _employeesService.Add(employee);
                });
            }
            catch
            {
                return new ResultModel<List<Company>> { State = ResultType.Error.ToString(), Message = "添加公司失败", Data = Companies };
            }
            return new ResultModel<List<Company>> { State = ResultType.Success.ToString(), Message = "添加公司成功", Data = Companies };


        }
        /// <summary>
        /// 查询 翻页+排序(公司和员工【两者都要有】）)
        /// </summary>
        /// <param name="pageSize">显示条数（需要前端定的）</param>
        /// <param name="pageindex">第几页<</param>
        /// <returns></returns>
        [HttpGet("GetCompaniesEmployeeInfo")]        
        public async Task<ResultModel<List<V_CompanyEmployeeInfo>>> GetCompaniesEmployeeInfo(int pageSize, int pageindex)
        {
            var accesstoken = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
            UserInfo = _iCompaniesService.GetLoginInfo(accesstoken);
            int t = 0;
            Expression<Func<V_CompanyEmployeeInfo, bool>> exp = c => true;
            List<V_CompanyEmployeeInfo> result = _iv_CompanyEmployeeInfoServices.Query(exp, pageindex, pageSize, "", out t).ToList();
            return new ResultModel<List<V_CompanyEmployeeInfo>> { State = ResultType.Success.ToString(), Message = "查询信息成功", Data = result };
        }

        /// <summary>
        /// 获取所有公司，根据名称模糊查询相关公司
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCompanies")]
        public async Task<ResultModel<List<Company>>>GetCompanies(int pageSize, int pageindex,string querycompanyName,string oderyFont)
        {
            try
            {
                var result = new List<Company>();
                int t = 0;
                if (querycompanyName == "" || querycompanyName == null)
                {
                    var accesstoken = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
                    UserInfo = _iCompaniesService.GetLoginInfo(accesstoken);
                    //int t = 0;
                    Expression<Func<Company, bool>> exp = c => true;
                    result = _iCompaniesService.Query(exp, pageindex, pageSize, oderyFont, out t).ToList();
                }
                else
                {
                    result = await _iCompaniesService.Query(x => x.Name.Contains(querycompanyName));
                    return new ResultModel<List<Company>> { State = ResultType.Success.ToString(), Message = "查询公司信息成功", Data = result };
                }
                return new ResultModel<List<Company>> { State = ResultType.Success.ToString(), Message = "查询公司信息成功", Data = result, Total = t };
            }
            catch (Exception)
            {
                return new ResultModel<List<Company>> { State = ResultType.Error.ToString(), Message = "查询公司信息失败"};
            }           
        }
        /// <summary>
        /// 根据公司id删除 公司和其下的员工
        /// </summary>
        /// <param name="CompanyIds"></param>
        /// <returns></returns>

        [HttpPost("Delete")]
        public async Task<ResultModel<List<Company>>> Delete([FromBody] Guid[] CompanyIds)
        {
            if (UserInfo.Role == "管理员")
            {
                try
                {
                    CompanyIds.ToList().ForEach(async i =>
                    {
                        //因为做了外键关联必须先删除子表，再删除主表的
                        var employees = _employeesService.Query(x => x.CompanyId == i);
                        employees.Result.ForEach(o =>
                        {
                            _employeesService.DeleteEntity(o, x => true);
                        });
                        await _iCompaniesService.DeleteById(i);
                    });
                    return new ResultModel<List<Company>> { State = ResultType.Success.ToString(), Message = "删除成功" };
                }
                catch (Exception e)
                {
                    return new ResultModel<List<Company>> { State = ResultType.Error.ToString(), Message = "删除失败" };
                }
            }
            return new ResultModel<List<Company>> { State = ResultType.Error.ToString(), Message = "没有相对于的权限" };
        }
       /// <summary>
       /// 编辑数据
       /// </summary>
       /// <param name="company"></param>
       /// <returns></returns>
       [HttpPost("Update")]
        public async Task<ResultModel<Company>> Update([FromBody] Company company)
        {
            if (UserInfo.Role == "管理员")
            {
                try
                {
                    await _iCompaniesService.Update(company,x=>x.Id==company.Id);
                    return new ResultModel<Company> { State = ResultType.Success.ToString(), Message = "更新公司信息成功" };
                }
                catch (Exception e)
                {
                    return new ResultModel<Company> { State = ResultType.Error.ToString(), Message = "更新公司信息失败" };
                }
            }
            return new ResultModel<Company> { State = ResultType.Error.ToString(), Message = "没有相对于的权限" };
        }
        /// <summary>
        /// 查询指定的数据
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet("UpdateId")]
        public async Task<ResultModel<Company>> UpdateId(Guid companyId)
        {
            if (UserInfo.Role == "管理员")
            {
                try
                {
                    var UpdateDate= await _iCompaniesService.QueryFirst(x => x.Id == companyId);
                    return new ResultModel<Company> { State = ResultType.Success.ToString(), Message = "查询当前数据成功", Data = UpdateDate };
                }
                catch (Exception e)
                {
                    return new ResultModel<Company> { State = ResultType.Error.ToString(), Message = "查询当前数据成功" };
                }
            }
            return new ResultModel<Company> { State = ResultType.Error.ToString(), Message = "没有相对于的权限" };
        }
    }
}
