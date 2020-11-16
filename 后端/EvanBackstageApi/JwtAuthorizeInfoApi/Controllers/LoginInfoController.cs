using EvanBackstageApi.Entity.JwtAuthorizeInfo;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Base;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.JwtViewMapperDtoModel.UserProfiles;
using EvanBackstageApi.Extensions.AuthHelper.OverWrite;
using EvanBackstageApi.IService.IJwtAuthorizeInfoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthorizeInfoApi.Controllers
{
    [ApiController]
    [Area("UserInfo")]
    [Route("api/[area]/[controller]")]
    public class LoginInfoController : ControllerBase
    {
        private readonly IUserService _userservices;
        public LoginInfoController(IUserService userServices)
        {
            _userservices = userServices;
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<object> Login(V_UserLoginDto user)
        {
            var result =await _userservices.Login(user);
            if(result.State== "success")
            {
                var vuser = result.Data;
                //权限
                var role = vuser.Roles.Select(r => r.Code);
                var token = new TokenModelJwt { Uid = result.Data.Id.ToString(), Role = string.Join(',', role) };
                var jwtStr = JwtHelper.IssueJwt(token);
                var suc = true;
                return new JwtResultModel<dynamic>
                {
                    State = "success",
                    Message = result.Message,
                    Data = new
                    {
                        vuser=result.Data,
                        jwttoken=jwtStr
                    }
                };
            }
            else
            {
                return new JwtResultModel<dynamic>
                {
                    State ="error",
                    Message = result.Message,
                    Data = null
                };
            }
            #region 老张教程的
            // 将用户id和角色名，作为单独的自定义变量封装进 token 字符串中。
            //TokenModelJwt tokenModel = new TokenModelJwt { Uid = 1, Role = "Admin" };
            //var jwtStr = JwtHelper.IssueJwt(tokenModel);//登录，获取到一定规则的 Token 令牌
            //var suc = true;
            //return Ok(new
            //{
            //    success = suc,
            //    token = jwtStr
            //});
            #endregion
        }
        [HttpGet]
        //[Authorize(Policy = "SystemOrAdmin")] 方法1
        [Authorize(Policy = "CustomizePolicy")]       
        public ActionResult<IEnumerable<string>> Get()
        {
            var t = User;
            return new string[] { "value1", "value2" };
        }
    }
}
