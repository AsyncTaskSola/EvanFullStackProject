using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace EvanBackstageApi.IRepository.IJwtAuthorizeInfoRepository
{
    public interface IUser
    {
        Guid Id { get; }
        IEnumerable<Claim> GetClaimsIdentity();
        List<string> GetClaimValueByType(string ClaimType);
    }
}
