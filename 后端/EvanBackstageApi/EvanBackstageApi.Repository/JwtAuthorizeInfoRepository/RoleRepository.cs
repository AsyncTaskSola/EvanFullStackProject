using EvanBackstageApi.Entity.JwtAuthorizeInfo.Root;
using EvanBackstageApi.IRepository;
using EvanBackstageApi.IRepository.IJwtAuthorizeInfoRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvanBackstageApi.Repository.JwtAuthorizeInfoRepository
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
