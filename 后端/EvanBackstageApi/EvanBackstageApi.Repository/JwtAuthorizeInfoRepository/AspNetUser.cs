using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using EvanBackstageApi.IRepository.IJwtAuthorizeInfoRepository;
using Microsoft.AspNetCore.Http;

namespace EvanBackstageApi.Repository.JwtAuthorizeInfoRepository
{
    public  class AspNetUser:IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AspNetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public Guid Id => new Guid(GetClaimValueByType("jti").FirstOrDefault());

        public List<string> GetClaimValueByType(string ClaimType)
        {
            var result= GetClaimsIdentity().Where(x => x.Type == ClaimType).Select(x => x.Value).ToList();
            return result;
        }
        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext.User.Claims;
        }
    }
}
