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
using System.Net.Http;
using System.Threading.Tasks;

namespace EvanBackstageApi.Controllers.CEGC
{
    [ApiController]
    [Area("CEGC")]
    [Route("api/[area]/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private static ILog log = LogManager.GetLogger(Startup.Repository.Name, "CompaniesController");
        private readonly ICompaniesService _iCompaniesService;
        private readonly IEmployeesService _employeesService;
        private readonly IV_CompanyEmployeeInfoServices _iv_CompanyEmployeeInfoServices;
        public LoginUserInfo UserInfo;
        
        public CompaniesController(ICompaniesService companiesService,
            IEmployeesService employeesService,
            IV_CompanyEmployeeInfoServices v_CompanyEmployeeInfoServices)
        {
            _iCompaniesService = companiesService;
            _employeesService = employeesService;
            _iv_CompanyEmployeeInfoServices = v_CompanyEmployeeInfoServices;
            log.Info("进入到控制器CompaniesController");
        }
        /// <summary>
        /// 添加公司 Id可以不填，后台自动生成
        /// </summary>
        /// <param name="Companies"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<ResultModel<List<Company>>> Add([FromBody] List<Company> Companies)
        {        
            if (UserInfo.Role == "管理员")
            {
                try
                {
                    Companies.ForEach(async item =>
                    {
                        if (item.Name == null) throw new Exception("公司名必填");
                        var employee = item.Emplyees as List<Employee>;
                        var result = await _iCompaniesService.Add(Companies);
                        _employeesService.Add(employee);                       
                    });
                }
                catch
                {
                    return new ResultModel<List<Company>> { State = ResultType.Error.ToString(), Message = "添加失败", Data = Companies };
                }
                return new ResultModel<List<Company>> { State = ResultType.Success.ToString(), Message = "添加成功", Data = Companies };
            }
            return new ResultModel<List<Company>> { State = ResultType.Error.ToString(), Message = "没有相对于的权限", Data = Companies };
        }
        /// <summary>
        /// 获取companies和其下的员工数据,等于原来的GetToEdit
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCompanies")]
        public async Task<ResultModel<List<V_CompanyEmployeeInfo>>> GetCompanies()
        {
            var accesstoken = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
            UserInfo = _iCompaniesService.GetLoginInfo(accesstoken);
            var result= await _iv_CompanyEmployeeInfoServices.Query();
            return new ResultModel<List<V_CompanyEmployeeInfo>> { State = ResultType.Success.ToString(), Message = "查询成功" ,Data=result};
        }

        [Authorize]
        [HttpPost("Delete")]
        public async Task<ResultModel<List<Company>>> Delete([FromBody] List<Guid> CompanyIds)
        {
            try
            {
                CompanyIds.ForEach(async i =>
                {
                    //因为做了外键关联必须先删除子表，再删除主表的
                    var employees = await _employeesService.Query(x => x.CompanyId == i);
                    var companyids = employees.Select(x => x.CompanyId).ToList();
                    await _employeesService.DeleteByIds(companyids);
                    await _iCompaniesService.DeleteById(i);
                });
                return new ResultModel<List<Company>> { State = ResultType.Success.ToString(), Message = "删除成功" };
            }
            catch(Exception ex)
            {
                return new ResultModel<List<Company>> { State = ResultType.Error.ToString(), Message = "删除失败" };
            }
        }


    }
}
