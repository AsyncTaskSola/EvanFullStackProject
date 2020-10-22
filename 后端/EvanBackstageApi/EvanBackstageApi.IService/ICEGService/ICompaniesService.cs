using EvanBackstageApi.Entity.CEG;
using EvanBackstageApi.Entity.UserInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvanBackstageApi.IService.ICEGService
{
    public interface ICompaniesService : IBaseService<Company>
    {
        LoginUserInfo GetLoginInfo(string accessToken);
    }
}
