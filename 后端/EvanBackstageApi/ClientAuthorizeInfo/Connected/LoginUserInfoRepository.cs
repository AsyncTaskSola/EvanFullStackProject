
using EvanBackstageApi.Entity.UserInfo;
using EvanBackstageApi.IRepository;
using EvanBackstageApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAuthorizeInfo.Connected
{
    public class LoginUserInfoRepository: BaseRepository<LoginUserInfo>, ILoginUserInfoRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public LoginUserInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
           
        }
        public void Create(bool Backup = false, int StringDefaultLength = 100, params Type[] types)
        {
            _unitOfWork.CreateTable(false, 100, types);
        }
    }
 
}
