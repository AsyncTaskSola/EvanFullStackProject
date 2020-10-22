
using EvanBackstageApi.Entity.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAuthorizeInfo.Connected
{
    public class LoginUserInfoRepository: BaseRepository<LoginUserInfo>, ILoginUserInfoRepository
    {
        public LoginUserInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
 
}
