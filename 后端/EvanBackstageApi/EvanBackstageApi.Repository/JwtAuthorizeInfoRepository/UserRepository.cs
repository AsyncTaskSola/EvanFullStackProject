using EvanBackstageApi.Entity.JwtAuthorizeInfo.Root;
using EvanBackstageApi.IRepository;
using EvanBackstageApi.IRepository.IJwtAuthorizeInfoRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvanBackstageApi.Repository.JwtAuthorizeInfoRepository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
