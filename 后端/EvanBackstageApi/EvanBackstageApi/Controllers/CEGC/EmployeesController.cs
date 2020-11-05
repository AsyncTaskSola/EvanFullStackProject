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
    
    [Area("CEGC")]
    [ApiController]
    [Authorize]
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
        [HttpGet("GetEmployees")]
        public async Task<ResultModel<List<Employee>>> GetEmployees(int pageSize, int pageindex, string queryEmployeeName)
        {
            try
            {
                int t = 0;
                var result = new List<Employee>();
                if (queryEmployeeName == "" || queryEmployeeName == null)
                {                  
                    var accesstoken = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
                    UserInfo = _iCompaniesService.GetLoginInfo(accesstoken);
                    Expression<Func<Employee, bool>> exp = c => true;
                    result = _employeesService.Query(exp, pageindex, pageSize, "", out t).ToList();
                    result.ForEach(x =>
                    {
                        var data = _iCompaniesService.QueryFirst(w => w.Id == x.CompanyId).Result;
                        x.CompanyName = data.Name;
                    });
                }
                else
                {
                    result = await _employeesService.Query(x => x.FirstName.Contains(queryEmployeeName));
                    result.ForEach(x =>
                    {
                        var data = _iCompaniesService.QueryFirst(w => w.Id == x.CompanyId).Result;
                        x.CompanyName = data.Name;
                    });
                    return new ResultModel<List<Employee>> { State = ResultType.Success.ToString(), Message = "查询员工信息成功", Data = result };
                }
                return new ResultModel<List<Employee>> { State = ResultType.Success.ToString(), Message = "查询员工信息成功", Data = result, Total = t };
            }
            catch (Exception)
            {
                return new ResultModel<List<Employee>> { State = ResultType.Error.ToString(), Message = "查询员工信息失败" };
            }
        }
        /// <summary>
        /// 更具id删除员工（支持批量删除）
        /// </summary>
        /// <param name="EmployeeIds"></param>
        /// <returns></returns>
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
                return new ResultModel<Company> { State = ResultType.Success.ToString(), Message = "查询员工信息成功",Data=result };
            }
            catch (Exception e)
            {
                return new ResultModel<Company> { State = ResultType.Success.ToString(), Message = "查询员工信息失败" };
            }
        }
        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("Add")]    
        public async Task<ResultModel<List<Employee>>> Add([FromBody] List<Employee> Employees)
        {
            try
            {
                Employees.ForEach(x =>
                {
                    x.Id = Guid.NewGuid();
                    //if (x.CompanyId == null) throw new Exception("公司ID必填"); 不建议重复查询，直接在后台去判断算了

                    //名字可以相同，公司的员工名有可能相同
                    //var data = await _employeesService.QueryFirst(i => i.FirstName == x.FirstName);
                    //if (data != null)
                    //{
                    //    throw new Exception("不能添加相同公司名");
                    //}

                    //假设前端只写了公司名
                    if (x.CompanyName == null || x.CompanyName == "") throw new Exception("公司名必填");
                    var data =  _iCompaniesService.QueryFirst(o => o.Name == x.CompanyName).Result;
                    if(data==null) throw new Exception("没有找到指定公司");
                    x.CompanyId= data.Id;
                    _employeesService.Add(Employees);
                });             
            }
            catch
            {
                return new ResultModel<List<Employee>> { State = ResultType.Error.ToString(), Message = "添加员工失败", Data = Employees };
            }
            return new ResultModel<List<Employee>> { State = ResultType.Success.ToString(), Message = "添加员工成功", Data = Employees };
        }

        /// <summary>
        /// 根据名称模糊查询相关员工
        /// </summary>
        /// <param name="employeeName"></param>
        /// <returns></returns>

        //[HttpGet("QueryCompany")]
        //public async Task<ResultModel<List<Employee>>> Upload(string employeeName)
        //{
        //    try
        //    {
        //        var result = await _employeesService.Query(x => x.FirstName.Contains(employeeName)||x.LastName.Contains(employeeName));
        //        return new ResultModel<List<Employee>> { State = ResultType.Success.ToString(), Message = "查询成功", Data = result };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultModel<List<Employee>> { State = ResultType.Error.ToString(), Message = "查询失败" };
        //    }
        //}
    }
}
