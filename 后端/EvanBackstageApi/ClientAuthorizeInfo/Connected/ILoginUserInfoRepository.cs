
using EvanBackstageApi.Entity.UserInfo;
using EvanBackstageApi.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAuthorizeInfo.Connected
{
    public interface ILoginUserInfoRepository : IBaseRepository<LoginUserInfo>
    {
        void Create(bool Backup = false, int StringDefaultLength = 100, params Type[] types);
    }
}
