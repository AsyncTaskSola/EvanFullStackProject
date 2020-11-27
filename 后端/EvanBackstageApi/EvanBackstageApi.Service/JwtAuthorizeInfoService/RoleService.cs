using AutoMapper;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Root;
using EvanBackstageApi.IRepository;
using EvanBackstageApi.IRepository.IJwtAuthorizeInfoRepository;
using EvanBackstageApi.IService.IJwtAuthorizeInfoService;
using EvanBackstageApi.Repository.JwtAuthorizeInfoRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Base;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.JwtViewMapperDtoModel.RoleProfiles;

namespace EvanBackstageApi.Service.JwtAuthorizeInfoService
{
    public  class RoleService: BaseService<Role>, IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleRepository _dal;
        private readonly IMapper _mapper;
        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _dal = new RoleRepository(unitOfWork);
            _mapper = mapper;
            BaseDal = _dal;
        }

    }
}
