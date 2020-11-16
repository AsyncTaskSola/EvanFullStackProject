using EvanBackstageApi.IService.IJwtAuthorizeInfoService;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthorizeInfoApi.PolicyRequirment
{
    /// <summary>
    /// 自定义策略授权
    /// </summary>
    //public class CustomizePolicyHandler : IAuthorizationHandler
    //{

    //    public Task HandleAsync(AuthorizationHandlerContext context)
    //    {
    //        var requirement = context.Requirements.FirstOrDefault();//当前接口上策略中的实体
    //        context.Succeed(requirement);
    //        return Task.CompletedTask;
    //    }
    //}


    public class CustomizePolicyHandler : AuthorizationHandler<CustomizePolicyRequirment>
    {
        private readonly IRoleService _roleServices;

        public CustomizePolicyHandler(IRoleService roleServices)
        {
            _roleServices = roleServices;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomizePolicyRequirment requirement)
        {
            //var name = context.User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Role)?.Value;
            //context.Succeed(requirement);
            //return Task.CompletedTask;
            var current = context.User.Claims.Where(c => c.Type == requirement.ClaimType).Select(c => c.Value).ToList();
            var res = requirement.Roles.Any();
            if(!requirement.Roles.Any())
            {
                var roles = _roleServices.Query(r => !r.IsDeleted).Result;
                requirement.Roles = roles;
            }
            var isMatchRole = requirement.Roles.Any(r => current.Contains(r.Code));

            if (isMatchRole)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return;
        }
    }
}
