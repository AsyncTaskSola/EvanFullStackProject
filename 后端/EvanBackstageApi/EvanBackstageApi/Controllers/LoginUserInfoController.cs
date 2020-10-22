using EvanBackstageApi.Basic;
using EvanBackstageApi.Entity.UserInfo;
using EvanBackstageApi.IRepository;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EvanBackstageApi.Controllers
{
    /// <summary>
    /// 单独处理不走仓储,客户登陆接口信息查看，限死
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginUserInfoController : ControllerBase
    {
        protected readonly ISqlSugarClient _db;
        private readonly IUnitOfWork _unitOfWork;
        public LoginUserInfoController(IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
            _db = _unitOfWork.GetDbClient();
        }
        [HttpPost("GetInfo")]
        public async Task<ResultModel<LoginUserInfo>> GetInfo()
        {
            var aa = HttpContext.User;
            var accesstoken= HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
            var result= _unitOfWork.GetInfo(accesstoken);
            //var result =await _db.Queryable<LoginUserInfo>().Where(x => x.AccessToken == accesstoken).FirstAsync();
            //var resultNew= await _db.Queryable<LoginUserInfo>().Where(x => x.AccessToken == accesstoken).OrderBy(x=>x.DateTimeStart,OrderByType.Desc).FirstAsync();
            //result.CurrentTime = resultNew.DateTimeStart;
            return new ResultModel<LoginUserInfo> { State = ResultType.Success.ToString(), Message = "查询成功", Data = result };
        }
    }
}
