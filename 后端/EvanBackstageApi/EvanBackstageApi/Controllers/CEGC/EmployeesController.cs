using EvanBackstageApi.Basic;
using EvanBackstageApi.Entity.CEG;
using EvanBackstageApi.Entity.UserInfo;
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
    [Route("api/[area]/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private static ILog log = LogManager.GetLogger(Startup.Repository.Name, "EmployeesController");
        private readonly ICompaniesService _iCompaniesService;
        private readonly IEmployeesService _employeesService;
        public static LoginUserInfo UserInfo;
        public EmployeesController(ICompaniesService companiesService,
         IEmployeesService employeesService,
         IV_CompanyEmployeeInfoServices v_CompanyEmployeeInfoServices)
        {
            _iCompaniesService = companiesService;
            _employeesService = employeesService;
            log.Info("进入到控制器EmployeesController");
        }
        /// <summary>
        /// 查询所有员工人员
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetEmployees")]
        public async Task<ResultModel<List<Employee>>> GetEmployees(int pageSize, int pageindex)
        {
            var accesstoken = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
            UserInfo = _iCompaniesService.GetLoginInfo(accesstoken);
            int t = 0;
            Expression<Func<Employee, bool>> exp = c => true;
            List<Employee> result = _employeesService.Query(exp, pageindex, pageSize, "", out t).ToList();
            return new ResultModel<List<Employee>> { State = ResultType.Success.ToString(), Message = "查询成功", Data = result };
        }
        /// <summary>
        /// 更具id删除员工（支持批量删除）
        /// </summary>
        /// <param name="EmployeeIds"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Delete")]
        public async Task<ResultModel<List<Employee>>> Delete([FromBody] Guid[] EmployeeIds)
        {
            if (UserInfo.Role == "管理员")
            {
                try
                {
                    await _employeesService.DeleteByIds(EmployeeIds.ToList());
                    return new ResultModel<List<Employee>> { State = ResultType.Success.ToString(), Message = "删除成功" };
                }
                catch (Exception)
                {
                    return new ResultModel<List<Employee>> { State = ResultType.Error.ToString(), Message = "删除失败" };
                }
            }
            return new ResultModel<List<Employee>> { State = ResultType.Error.ToString(), Message = "没有相对的权限" };
        }
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Upload")]
        public async Task<ResultModel<Employee>> Upload([FromBody] Employee employee)
        {
            if (UserInfo.Role == "管理员")
            {
                try
                {
                    await _employeesService.Update(employee, x => x.CompanyId == employee.CompanyId);
                    return new ResultModel<Employee> { State = ResultType.Success.ToString(), Message = "更新成功" };
                }
                catch (Exception e)
                {
                    return new ResultModel<Employee> { State = ResultType.Error.ToString(), Message = "更新失败" };
                }
            }
            return new ResultModel<Employee> { State = ResultType.Error.ToString(), Message = "没有相对于的权限" };
        }
        /// <summary>
        /// 根据id查询所在该员工所在的公司
        /// </summary>
        /// <param name="employeeid"></param>
        /// <returns></returns>
        [HttpGet("EcompanyInfo")]
        public async Task<ResultModel<Company>> EcompanyInfo(Guid employeeid)
        {
            try
            {
                var companyid = _employeesService.GetEcompanyInfo(x => x.Id == employeeid, o => o.CompanyId);
                var result= await _iCompaniesService.QueryFirst(x => x.Id == companyid);
                return new ResultModel<Company> { State = ResultType.Success.ToString(), Message = "查询成功",Data=result };
            }
            catch (Exception e)
            {
                return new ResultModel<Company> { State = ResultType.Success.ToString(), Message = "查询失败" };
            }
        }
        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Add")]    
        public async Task<ResultModel<List<Employee>>> Add([FromBody] List<Employee> Employees)
        {
            try
            {
                Employees.ForEach(async x =>
                {
                    x.Id = Guid.NewGuid();
                    if (x.CompanyId == null) throw new Exception("公司ID必填");
                    await _employeesService.Add(Employees);
                });             
            }
            catch
            {
                return new ResultModel<List<Employee>> { State = ResultType.Error.ToString(), Message = "添加失败", Data = Employees };
            }
            return new ResultModel<List<Employee>> { State = ResultType.Success.ToString(), Message = "添加成功", Data = Employees };
        }

        /// <summary>
        /// 根据名称模糊查询相关员工
        /// </summary>
        /// <param name="employeeName"></param>
        /// <returns></returns>

        [HttpGet("QueryCompany")]
        public async Task<ResultModel<List<Employee>>> Upload(string employeeName)
        {
            try
            {
                var result = await _employeesService.Query(x => x.FirstName.Contains(employeeName)||x.LastName.Contains(employeeName));
                return new ResultModel<List<Employee>> { State = ResultType.Success.ToString(), Message = "查询成功", Data = result };
            }
            catch (Exception ex)
            {
                return new ResultModel<List<Employee>> { State = ResultType.Error.ToString(), Message = "查询失败" };
            }
        }
    }
}
