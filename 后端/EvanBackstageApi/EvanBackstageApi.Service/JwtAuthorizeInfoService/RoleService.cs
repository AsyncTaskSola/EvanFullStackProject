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

        public async Task<JwtResultModel<dynamic>> UpdateUserRoleInfo(UserRole userRole)
        {
            try
            {
                var userisinit = await _unitOfWork.GetDbClient().Queryable<User>().Where(x => x.Id == userRole.UserId && x.IsInit).FirstAsync();
                if (userisinit != null)
                {
                    return new JwtResultModel<dynamic> { Message = "初始用户不能修改" };
                }
                var entity = await _unitOfWork.GetDbClient().Queryable<UserRole>().Where(x => x.UserId == userRole.UserId).FirstAsync();
                entity.RoleId = userRole.RoleId;
                await _unitOfWork.GetDbClient().Updateable<UserRole>(entity).ExecuteCommandAsync();
                return new JwtResultModel<dynamic> { Message = "更新成功", State = JwtResultType.Success };
            }
            catch (Exception)
            {
                return new JwtResultModel<dynamic> { Message = "更新失败", State = JwtResultType.Error };
            }
            
        }
    }
}
