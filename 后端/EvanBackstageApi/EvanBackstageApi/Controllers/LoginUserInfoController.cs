using EvanBackstageApi.Basic;
using EvanBackstageApi.Entity.UserInfo;
using EvanBackstageApi.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
            return new ResultModel<LoginUserInfo> { State = ResultType.Success.ToString(), Message = "查询成功", Data = result };
        }
        /// <summary>
        /// 查看所有登陆信息记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetLoginInfo")]
        public async Task<ResultModel<List<LoginUserInfo>>> GetLoginInfo(int pageSize, int pageindex, string oderyFont)
        {
            try
            {
                var resultNew = _db.Queryable<LoginUserInfo>().ToList();
                int t = _db.Queryable<LoginUserInfo>().Count();
                Expression<Func<LoginUserInfo, bool>> exp = c => true;
                List<LoginUserInfo> result = _db.Queryable<LoginUserInfo>()
               .OrderByIF(!string.IsNullOrEmpty(oderyFont), oderyFont)
               .WhereIF(exp != null, exp)
               .ToPageList(pageindex, pageSize);
                return new ResultModel<List<LoginUserInfo>> { State = ResultType.Success.ToString(), Message = "查询成功", Data = result, Total = t };
            }
            catch (Exception)
            {

                return new ResultModel<List<LoginUserInfo>> { State = ResultType.Error.ToString(), Message = "查询失败"};
            }
            
        }
    }
}
