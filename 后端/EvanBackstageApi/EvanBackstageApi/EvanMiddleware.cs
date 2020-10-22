using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace EvanBackstageApi
{
    /// <summary>
    /// 自定义中间件判断是否登陆到授权服务器，全局
    /// </summary>
    public class EvanMiddleware
    {
        private readonly RequestDelegate _next;

        public EvanMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            //var r= httpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
            //httpContext.User =;
            var result = httpContext.User.Identities.Select(x=>x.Name).FirstOrDefault();
            string displayUrl = httpContext.Request.Host.ToString()+ "/Home/AccessApi1";
            if (result==""||result==null)
            {
                throw new Exception($"请登陆授权服务器{displayUrl}");
            }           
            return _next(httpContext);
        }
    }
    public static class EvanMiddlewareExtensions
    {
        public static IApplicationBuilder UseEvanMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EvanMiddleware>();
        }
    }
}
