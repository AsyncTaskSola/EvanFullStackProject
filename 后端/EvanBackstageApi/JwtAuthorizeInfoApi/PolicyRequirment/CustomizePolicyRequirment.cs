using EvanBackstageApi.Entity.JwtAuthorizeInfo.Root;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthorizeInfoApi.PolicyRequirment
{
    public class CustomizePolicyRequirment:IAuthorizationRequirement
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public List<Role> Roles { get; set; }
        /// <summary>
        /// 认证授权类型
        /// </summary>
        public string ClaimType { internal get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public TimeSpan Expiration { get; set; }
    }
}
