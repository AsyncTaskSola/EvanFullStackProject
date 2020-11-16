using EvanBackstageApi.Entity.JwtAuthorizeInfo;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace JwtAuthorizeInfoApi.Controllers.UserInfo
{
    [ApiController]
    [Area("UserInfo")]
    [Route("api/[area]/[controller]")]
    public class UserInfoController : ControllerBase
    {
        #region 没有封装前
        //public IConfiguration Configuration { get; }
        //public UserInfoController(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}
        //[HttpPost("GetJwt")]
        //public string GetJwt([FromBody] TokenModel tokenModel)
        //{
        //    if (tokenModel.Name == "Evan" && tokenModel.PassWord == "123")
        //    {
        //        var clamins = new[]
        //        {
        //            new Claim(ClaimTypes.Name,tokenModel.Name),
        //            new Claim(ClaimTypes.Email,tokenModel.Email),
        //            new Claim(ClaimTypes.Role,tokenModel.Role),
        //            //new Claim("Delete","删除"),
        //            //new Claim("Add","添加"),
        //            new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddMinutes(Convert.ToInt32(Configuration.GetSection("JWT")["Expires"]))).ToUnixTimeSeconds()}"),
        //            new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}")
        //        };

        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JWT")["IssuerSigningKey"]));
        //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //        var securityToken = new JwtSecurityToken(
        //                issuer: Configuration.GetSection("JWT")["ValidIssuer"],
        //                audience: Configuration.GetSection("JWT")["ValidAudience"],
        //                claims: clamins,
        //                expires: DateTime.Now.AddMinutes(Convert.ToInt32(Configuration.GetSection("JWT")["Expires"])),//token有效时间30
        //                signingCredentials: creds);
        //        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        //        return token;
        //    }
        //    else
        //    {
        //        throw new Exception("账号密码错误");
        //    }
        //}

        //[Authorize(Roles = "管理员")]// 添加授权特性
        ///// <summary>
        ///// 认证通过之后可访问
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult<TokenModel> Get(TokenModel tokenModel)
        //{
        //    var t = User.Claims.Where(x=>x.Type== "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").FirstOrDefault().Value;
        //    var result= User.FindFirstValue(JwtClaimTypes.Role);
        //    return new TokenModel { ID = 1 };
        //}
        #endregion
    }
}
