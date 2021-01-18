using EvanBackstageApi.Entity.JwtAuthorizeInfo;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Base;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.JwtViewMapperDtoModel.UserProfiles;
using EvanBackstageApi.Extensions.AuthHelper.OverWrite;
using EvanBackstageApi.IService.IJwtAuthorizeInfoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Root;
using EvanBackstageApi.Repository;

namespace JwtAuthorizeInfoApi.Controllers
{
    [ApiController]
    [Area("UserInfo")]
    [Route("api/[controller]")]
    [Authorize("CustomizePolicy")]
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

        [HttpPost("Logout")]
        [AllowAnonymous]
        public async Task<JwtResultModel<dynamic>> Logout(Guid Id)
        {
            return await _userservices.Logout(Id);
        }

        [HttpPost("update")]
        [AllowAnonymous]
        public async Task<ActionResult<JwtResultModel<V_UserDto>>> Update([FromForm] V_UserUpdateDto update)
        {
            return await _userservices.Update(update);
        }

        [HttpPost("Delete")]
        public async Task<JwtResultModel<dynamic>> Delete(Guid Id)
        {
            return await _userservices.DeleteId(Id);
        }


        [HttpPost("Add")]
        public async Task<JwtResultModel<V_UserDto>> Add(/*[FromForm]*/V_UserAddDto V_user)
        {
            return await _userservices.AddUser(V_user);
        }

        [HttpGet("GetUserById")]
        public async Task<JwtResultModel<V_SysUserDto>> GetUserById(Guid userid)
        {
            var result= await _userservices.CheckUserInfo(userid);
            return result;
        }

        [HttpGet("GetUsers")]
        [AllowAnonymous]
        public async Task<JwtResultModel<List<V_UserDto>>> GetUsers(int pageSize, int pageindex, string oderyFont)
        {
            var t = 0;
            pageSize = 20;
            Expression<Func<User, bool>> exp = c => true;
            var uselist = _userservices.Query(exp, pageindex, pageSize, oderyFont, out t).ToList();
            uselist = uselist.Where(x => x.IsDeleted == false).ToList();
            var result= await _userservices.Mapperdata(uselist);
            result.Total = uselist.Count;
            return result;
        }
        /// <summary>
        /// 是否被停用
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpPost("Disable")]
        public async Task<JwtResultModel<dynamic>> Disable(Guid userid)
        {
            //判断
           return await _userservices.DisableUser(userid);
        }

        /// <summary>
        /// 密码重设，默认登陆密码为123456 | [前端若登陆次数满了，可以选择忘记密码]
        /// </summary>
        /// <returns></returns>
        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<JwtResultModel<dynamic>> ResetPassword(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var result = await _userservices.QueryFirst(x => x.Name == name || x.Account == name);
                if (result != null)
                {
                    result.Password = MD5Helper.Md5Str("123455");
                    result.Status = false;
                    result.IsDisable = false;
                    await _userservices.Update(result);

                    return new JwtResultModel<dynamic> {Message = "重设密码成功（密码为123456）", State = JwtResultType.Success};
                }
                return new JwtResultModel<dynamic> { Message = "重设失败，请输入对应的用户名和账号id", State = JwtResultType.Error };
            }
            return new JwtResultModel<dynamic> { Message = "重设失败，请输入对应的用户名和账号id", State = JwtResultType.Error };
        }

        //[HttpGet]
        ////[Authorize(Policy = "SystemOrAdmin")] 方法1
        //[Authorize(Policy = "CustomizePolicy")]       
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    var t = User;
        //    return new string[] { "value1", "value2" };
        //}
    }
}
