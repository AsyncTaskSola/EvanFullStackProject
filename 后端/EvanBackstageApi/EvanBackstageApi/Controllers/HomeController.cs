using EvanBackstageApi.Basic;
using IdentityModel.Client;
using log4net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace EvanBackstageApi.Controllers
{
    #region 另外写了，jwt问题
    //public class HomeController : ControllerBase
    //{
    //    private static ILog log = LogManager.GetLogger(Startup.Repository.Name, "HomeController");
    //    public HomeController()
    //    {
    //        log.Info("进入到控制器HomeController");
    //    }
    //    //public ResultModel<IIdentity> Index()
    //    //{
    //    //    var user = User.Identity;
    //    //    return new ResultModel<IIdentity> { State = ResultType.Success.ToString(), Message = "成功",Data=user};
    //    //}

    //    [Authorize]
    //    public async Task<ResultModel<Dictionary<string, object>>> AccessApi1()
    //    {
    //        var result = User.IsInRole("管理员");
    //        var info = Run();
    //        var client = new HttpClient();
    //        var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5001");
    //        if (disco.IsError)
    //        {
    //            throw new Exception();
    //        }
    //        var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
    //        client.SetBearerToken(accessToken);

    //        return new ResultModel<Dictionary<string, object>> { State = ResultType.Success.ToString(), Message = "成功", Data = info };
    //    }
    //    public Dictionary<string, object> Run()
    //    {
    //        string PrivacyUrl = HttpContext.Request.Host.ToString() + "/Home/Privacy";
    //        var listinfo = new Dictionary<string, object>();
    //        var user = User.Identity;
    //        var IsAuthenticated = user.IsAuthenticated;
    //        var AuthenticationType = user.AuthenticationType;
    //        var username = User.Identities.Select(x => x.Name).First();
    //        var roleinfo = User.Identities.Select(x => x.Claims).First().Select(x => x.Value).ToList();
    //        var role = "";
    //        try
    //        {
    //            role = roleinfo[8];
    //        }
    //        catch (Exception)
    //        {
    //            role = "";
    //        }

    //        listinfo.Add("IsAuthenticated", IsAuthenticated);
    //        listinfo.Add("AuthenticationType", AuthenticationType);
    //        listinfo.Add("username", username);
    //        listinfo.Add("role", role);
    //        listinfo.Add("获取登陆信息(仅管理员查看)", PrivacyUrl);
    //        return listinfo;
    //    }
    //    /// <summary>
    //    /// 获取登陆信息
    //    /// </summary>
    //    /// <returns></returns>
    //    [Authorize("管理员")]
    //    public async Task<Dictionary<string, string>> Privacy()
    //    {
    //        var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
    //        var IdToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
    //        var RefreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
    //        //var AuthorizationCode = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.Code);
    //        var logininfo = new Dictionary<string, string>();
    //        logininfo.Add("accessToken", accessToken);
    //        logininfo.Add("IdToken", IdToken);
    //        logininfo.Add("RefreshToken", RefreshToken);
    //        return logininfo;
    //    }

    //    public async Task Lagout()
    //    {

    //        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    //        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
    //    }
    //}
    #endregion
}
