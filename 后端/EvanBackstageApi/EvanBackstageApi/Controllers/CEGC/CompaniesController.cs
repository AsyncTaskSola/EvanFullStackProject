using EvanBackstageApi.Basic;
using EvanBackstageApi.Entity.CEG;
using EvanBackstageApi.Entity.UserInfo;
using EvanBackstageApi.IService.ICEGService;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvanBackstageApi.Controllers.CEGC
{
    [Area("CEGC")]
    [Route("api/[area]/[controller]")]
    public class CompaniesController: ControllerBase
    {
        private static ILog log = LogManager.GetLogger(Startup.Repository.Name, "CompaniesController");
        private readonly ICompaniesService _iCompaniesService;
        public LoginUserInfo UserInfo;
        public CompaniesController(ICompaniesService companiesService)
        {
            _iCompaniesService = companiesService;
            UserInfo= _iCompaniesService.GetLoginInfo(HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1]);
            log.Info("进入到控制器CompaniesController");
        }
        /// <summary>
        /// 添加公司 Id可以不填，后台自动生成
        /// </summary>
        /// <param name="Companies"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost("Add")]
        [Authorize]
        public async Task<ResultModel<List<Company>>> Add([FromBody] List<Company> Companies)
        {         
            if(UserInfo.Role=="管理员")
            {
                try
                {
                    Companies.ForEach(async item =>
                    {
                        if (item.Name == null) throw new Exception("公司名必填");
                        var employee = item.Emplyees;
                        var result = await _iCompaniesService.Add(Companies);
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
    }
}
