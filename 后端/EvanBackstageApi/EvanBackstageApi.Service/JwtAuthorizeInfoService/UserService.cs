using AutoMapper;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Base;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.JwtViewMapperDtoModel.RoleProfiles;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.JwtViewMapperDtoModel.UserProfiles;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Root;
using EvanBackstageApi.IRepository;
using EvanBackstageApi.IRepository.IJwtAuthorizeInfoRepository;
using EvanBackstageApi.IService.IJwtAuthorizeInfoService;
using EvanBackstageApi.Repository;
using EvanBackstageApi.Repository.JwtAuthorizeInfoRepository;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvanBackstageApi.Service.JwtAuthorizeInfoService
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _dal;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _dal = new UserRepository(unitOfWork);
            BaseDal = _dal;
            _mapper = mapper;
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public async Task<JwtResultModel<V_UserDto>> Login(V_UserLoginDto User)
        {
            try
            {
                var result =await _dal.QueryFirst(x => x.Account == User.Account && !x.IsDeleted);
                if (result == null)
                {
                    return new JwtResultModel<V_UserDto> { Message = "账号不存在" };
                }
                var IsDisable =await _dal.QueryFirst(x => x.Account == User.Account && !x.IsDisable);
                if (IsDisable == null)
                {
                    return new JwtResultModel<V_UserDto> { Message = "已停用" };
                }


                var user = await _dal.QueryFirst(x => x.Account == User.Account);
                user.LastDate = DateTime.Now;
                await _dal.Update(user);
                // 尝试登陆时间小于上次登陆时间
                if (user.AgainDate < user.LastDate)
                {
                    user.ErrorCount = 0;
                    user.AgainDate = user.LastDate;
                    await _dal.Update(user);
                }
                //账号是否登陆了
                if (user.Status)
                {
                    return new JwtResultModel<V_UserDto> { Message = "账号已登陆" };
                }
                //登陆次数判断
                if (user.ErrorCount == 3)
                {
                    return new JwtResultModel<V_UserDto> { Message = "账号超过登陆最大尝试次数，被锁定" };
                }
                var UsererrPwd = await _dal.QueryFirst(u => u.Account == User.Account && u.Password == MD5Helper.Md5Str(User.Password));
                if (UsererrPwd == null)
                {
                    user.ErrorCount += 1;
                    user.AgainDate = user.LastDate.AddMinutes(2);
                    await _dal.Update(user);
                    return new JwtResultModel<V_UserDto> { Message = "密码不正确" };
                }
                else
                {
                    //启用原生自带的程序方法，不再全部集成在仓储去调用,仓储不应该涉及业务，底层只会用于通用
                    var db = _unitOfWork.GetDbClient();
                    var rolesid = await db.Queryable<UserRole, User>((usr, us) => new object[]
                           {
                               JoinType.Left,usr.UserId==us.Id,
                           }).Select(usr => usr.RoleId).FirstAsync();
                    var roles = await db.Queryable<Role>().Where(x => x.Id == rolesid).ToListAsync();
                    var vuser = _mapper.Map<V_UserDto>(user);
                    var vrole = _mapper.Map<List<V_RoleDto>>(roles);
                    vuser.Roles = vrole;
                    //登陆成功应该把错误次数重设为0
                    user.ErrorCount = 0;
                    user.Status = true;
                    user.AgainDate = DateTime.Now;
                    await BaseDal.Update(user);
                    return new JwtResultModel<V_UserDto> { Message = "登陆成功",State= JwtResultType.Success.ToString(), Data= vuser };
                };
            }
            catch (Exception ex)
            {
                return new JwtResultModel<V_UserDto> { Message = "登陆失败", State = JwtResultType.Error.ToString() };
            }
        }
    }
}
