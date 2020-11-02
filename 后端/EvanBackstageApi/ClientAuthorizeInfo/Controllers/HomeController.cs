
using ClientAuthorizeInfo.Connected;
using EvanBackstageApi.Basic;
using EvanBackstageApi.Entity.UserInfo;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClientAuthorizeInfo.Controllers
{
    public class HomeController : Controller
    {

        LoginUserInfo loginUserInfo = new LoginUserInfo();
        private readonly ILoginUserInfoRepository _LoginUserInfoRepository;
        public HomeController(ILoginUserInfoRepository LoginUserInfoRepository)
        {

            _LoginUserInfoRepository = LoginUserInfoRepository;
            //_LoginUserInfoRepository.Create(false,100,typeof(LoginUserInfo));
        }
        /// <summary>
        /// 与授权服务器交互，获取accessToken 访问Api是否有权限操作
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<ResultModel<Dictionary<string, object>>> AccessApi1()
        {
            var info = new Dictionary<string,object>();
            try
            {
                var result = User.IsInRole("管理员");
                Expression<Func<LoginUserInfo, bool>> exp = w => true;
                Expression<Func<LoginUserInfo, bool>> exp2 = x => true;
                var client = new HttpClient();
                var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5001");
                if (disco.IsError)
                {
                    throw new Exception();
                }
                info = Run();
                loginUserInfo.AccessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
                loginUserInfo.DateTimeStart = DateTime.Now;
                loginUserInfo.ID = Guid.NewGuid();
                info.Add("accessToken", loginUserInfo.AccessToken);
                client.SetBearerToken(loginUserInfo.AccessToken);
                var response = await client.GetAsync("http://localhost:6001/identity");
                if (!response.IsSuccessStatusCode)
                {

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        info.Add("IsAuthenticated", loginUserInfo.IsAuthenticated);
                        var newvule = await RenewTokensAsync();
                        loginUserInfo.RefreshToken = newvule.Split('|')[0];
                        loginUserInfo.AccessToken = newvule.Split('|')[1];
                        info.Remove("accessToken");
                        info.Add("重新获取accessToken", loginUserInfo.AccessToken);
                        info.Add("获取RefreshToken", loginUserInfo.RefreshToken);
                        await _LoginUserInfoRepository.Add(loginUserInfo);
                        //在获取新的token时候应该把原有的accesstoken状态更新为过期也就是loginUserInfo.IsAuthenticated:flase 来做判断                
                        var listuserinfo = await _LoginUserInfoRepository.Query(exp, x => x.DateTimeStart, false);
                        exp = x => x.ID != loginUserInfo.ID;
                        exp2 = w => w.IsAuthenticated == false;
                        await _LoginUserInfoRepository.Update(listuserinfo, exp, exp2);
                        return new ResultModel<Dictionary<string, object>> { State = ResultType.Success.ToString(), Message = "成功", Data = info };
                    }
                    throw new Exception(response.ReasonPhrase);
                }
                else
                {
                    info.Add("IsAuthenticated", loginUserInfo.IsAuthenticated);
                    //是否再次插入应该根据数据库是否包含这条AccessToken，保证唯一
                    var results = await _LoginUserInfoRepository.Query(x => x.AccessToken == loginUserInfo.AccessToken);
                    if(results.Count==0)
                    {
                        await _LoginUserInfoRepository.Add(loginUserInfo);
                    }                 
                }
            }
            catch (Exception)
            {
                return new ResultModel<Dictionary<string, object>>
                {
                    State = ResultType.Success.ToString(),
                    Message = "失败"
                };
            }
            return new ResultModel<Dictionary<string, object>> { State = ResultType.Success.ToString(), Message = "成功", Data = info };
        }
        public Dictionary<string, object> Run()
        {
            loginUserInfo.PrivacyUrl = HttpContext.Request.Host.ToString() + "/Home/Privacy";
            var listinfo = new Dictionary<string, object>();
            var user = User.Identity;
            loginUserInfo.IsAuthenticated = user.IsAuthenticated;
            loginUserInfo.AuthenticationType = user.AuthenticationType;
            loginUserInfo.Username = User.Identities.Select(x => x.Name).First();
            var roleinfo = User.Identities.Select(x => x.Claims).First().Select(x => x.Value).ToList();
            loginUserInfo.Role = User.FindFirstValue(JwtClaimTypes.Role);
            listinfo.Add("AuthenticationType", loginUserInfo.AuthenticationType);
            listinfo.Add("username", loginUserInfo.Username);
            listinfo.Add("role", loginUserInfo.Role);
            listinfo.Add("获取登陆信息(仅管理员查看)", loginUserInfo.PrivacyUrl);
            return listinfo;
        }
        /// <summary>
        /// 获取登陆信息
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "管理员")]
        public async Task<Dictionary<string, string>> Privacy()
        {
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var IdToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            var RefreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            //var AuthorizationCode = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.Code);
            var logininfo = new Dictionary<string, string>();
            logininfo.Add("accessToken", accessToken);
            logininfo.Add("IdToken", IdToken);
            logininfo.Add("RefreshToken", RefreshToken);
            return logininfo;
        }

        public async Task Lagout()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        //现在设置了30分钟验证一次，默认是1小时的，刷新获取新的token 
        private async Task<string> RenewTokensAsync()
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5001");
            if (disco.IsError)
            {
                throw new Exception();
            }
            var RefreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            //获取新的toke用的  refresh token
            var tokenResponse = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                RefreshToken = RefreshToken,
                Address = disco.TokenEndpoint,
                ClientId = "hybrid client",
                ClientSecret = "hybrid secret",
                GrantType = OpenIdConnectGrantTypes.RefreshToken,
                Scope = "api1 openid profile email address phone roles locations"
            });
            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }
            else
            {
                //超时时间
                var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);
                var tokens = new[]
            {
                    new AuthenticationToken
                    {
                        Name=OpenIdConnectParameterNames.IdToken,
                        Value =tokenResponse.IdentityToken
                    },
                    new AuthenticationToken
                    {
                        Name=OpenIdConnectParameterNames.AccessToken,
                        Value =tokenResponse.AccessToken
                    },
                    new AuthenticationToken
                    {
                    Name=OpenIdConnectParameterNames.RefreshToken,
                    Value =tokenResponse.RefreshToken
                    },
                    new AuthenticationToken
                    {
                        Name="expires_at",
                        Value =expiresAt.ToString("o",CultureInfo.InvariantCulture)

                    }
                };
                //获取身份认证的结果，包含的当前的Principal，Properties
                var currentAuthenticateResult =
                    await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                //把新的token存起来
                currentAuthenticateResult.Properties.StoreTokens(tokens);
                //登陆
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    currentAuthenticateResult.Principal, currentAuthenticateResult.Properties);

                return tokenResponse.RefreshToken+'|'+tokenResponse.AccessToken;
            }
        }
    }
}
