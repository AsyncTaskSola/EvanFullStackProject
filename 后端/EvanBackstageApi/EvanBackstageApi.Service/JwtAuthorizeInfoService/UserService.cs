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
using Microsoft.AspNetCore.Hosting;
using JwtAuthorizeInfoApi.OtherHelp;

namespace EvanBackstageApi.Service.JwtAuthorizeInfoService
{
    public class UserService : BaseService<User>, IUserService
    {
        public IHostingEnvironment WebHostEnvironment { get; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _dal;
        private readonly IMapper _mapper;
        private readonly IUser _user;
        private readonly IBaseRepository<Role> _roledal;
        private readonly IBaseRepository<UserRole> _useroledal;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IHostingEnvironment webHostEnvironment,IUser user, IBaseRepository<Role> roledal, IBaseRepository<UserRole> useroledal)
        {
            WebHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
            _dal = new UserRepository(unitOfWork);
            BaseDal = _dal;
            _mapper = mapper;
            _user = user;
            _roledal = roledal;
            _useroledal = useroledal;
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
                           }).Where(usr => usr.UserId==user.Id).Select(usr => usr.RoleId).FirstAsync();
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

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<JwtResultModel<dynamic>> Logout(Guid id)
        {
            try
            {
                var result = _dal.QueryFirst(x => x.Id == id).Result;
                result.Status = false;
                await _dal.Update(result);
                return new JwtResultModel<dynamic> { Message = "退出成功", State = JwtResultType.Success };
            }
            catch (Exception e)
            {
                return new JwtResultModel<dynamic> { Message = "退出失败", State = JwtResultType.Error };
            }
      
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="UserUpdateDto"></param>
        /// <returns></returns>

        public async Task<JwtResultModel<V_UserDto>> Update(V_UserUpdateDto UserUpdateDto)
        {
            var user = await _dal.QueryFirst(x => x.Id == UserUpdateDto.Id);
            var updateColumns = new List<string>();
            if (UserUpdateDto.OldPassword == null || UserUpdateDto.NewPassword == null)
            {
                return new JwtResultModel<V_UserDto> { Message = "密码不能为空" };
            }
            if (UserUpdateDto.OldPassword!=user.Password)
            {
                return new JwtResultModel<V_UserDto> { Message = "请输入原有密码" };
            }
            updateColumns.Add("Password");
            var updateUser = _mapper.Map<User>(UserUpdateDto);//[User]为目标源，()为要更新的数据源
            var V_UserDto = _mapper.Map<V_UserDto>(UserUpdateDto);
            //修改头像
            if (UserUpdateDto.Avatar != null)
            {
                var avatarRes = await FileHelper.WriteAvatar(UserUpdateDto.Avatar, UserUpdateDto.Id);
                if (avatarRes.Success)
                {
                    updateColumns.Add("Avatar");
                    updateUser.Avatar = avatarRes.Message;
                    V_UserDto.Avatar= avatarRes.Message;
                }
            }
            //更新数据库
            var result =await _dal.Update(updateUser, updateColumns.ToArray());
            if (result)
            {
                return new JwtResultModel<V_UserDto> { Message = "更新成功",Data = V_UserDto};
            }
            return new JwtResultModel<V_UserDto> { Message = "更新失败"};
        }

        /// <summary>
        /// （假）删除 更新状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<JwtResultModel<dynamic>> DeleteId(Guid id)
        {
            var user =await _dal.QueryFirst(x => x.Id == id&&!x.IsDisable);
            if (user != null)
            {
                return new JwtResultModel<dynamic> { Message = "初始用户不能删除" };
            }

            if (user.Status)
            {
                return new JwtResultModel<dynamic> { Message = "当前用户在线不能删除" };
            }

            user.IsDeleted = true;
            var result =await _dal.Update(user, new string[] {"IsDeleted"});
            if (result)
            {
                return new JwtResultModel<dynamic> { Message = "删除成功"};
            }
            return new JwtResultModel<dynamic> { Message = "删除失败" };
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="V_adduser"></param>
        /// <returns></returns>
        public async Task<JwtResultModel<V_UserDto>> AddUser(V_UserAddDto V_adduser)
        {
            var userbool =await _dal.QueryFirst(x => x.Account == V_adduser.Account||x.Name== V_adduser.Name);
            if(userbool!=null)
            {
                return new JwtResultModel<V_UserDto> { Message = "账号或用户名已存在", State = JwtResultType.Error };
            }

            var user = _mapper.Map<User>(V_adduser);//<映射目标值>(要映射的数据)
            user.Id=Guid.NewGuid();
            user.Password = MD5Helper.Md5Str(user.Password);
            //创建名
            var createid = _user.Id;
            var createName =_dal.QueryFirst(x => x.Id == createid).Result.Name;
            user.Creator = createName;
            await _dal.Add(user);
            
            //权限
            var userroles = new List<UserRole>();
            var roles=new List<V_RoleDto>();
            V_adduser.Roles.ForEach(roleid =>
            {
                var role = _roledal.QueryFirst(x => x.Id == roleid).Result;
                var vrole = _mapper.Map<V_RoleDto>(role);
                var ur = new UserRole
                {
                    RoleId = roleid,
                    UserId = user.Id
                };
                userroles.Add(ur);
                roles.Add(vrole);
            });
            await _useroledal.Add(userroles);
            var v_user = _mapper.Map<V_UserDto>(user);
            v_user.Roles = roles;
            return new JwtResultModel<V_UserDto> { Message = "添加成功", State = JwtResultType.Success,Data = v_user};
        }

        /// <summary>
        /// 个人信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<JwtResultModel<V_SysUserDto>> CheckUserInfo(Guid id)
        {
            var user = await _dal.QueryFirst(x => x.Id == id);
            var sysuser = _mapper.Map<V_SysUserDto>(user);
            //获取权限组
            var roles =await GetRoles(user.Id);
            var vroles = _mapper.Map<List<V_RoleDto>>(roles);
            sysuser.Roles = vroles;
            return new JwtResultModel<V_SysUserDto> { Message = "查询成功", State = JwtResultType.Success,Data = sysuser};
        }

        public async Task<JwtResultModel<List<V_UserDto>>> Mapperdata(List<User> userList)
        {
            var vuser = _mapper.Map<List<V_UserDto>>(userList);
            vuser.ForEach(x =>
            {
                var userid = x.Id;
                var roles =  GetRoles(userid).Result;
                var vroles = _mapper.Map<List<V_RoleDto>>(roles);
                x.Roles = vroles;
            });
            return new JwtResultModel<List<V_UserDto>> { Message = "查询成功", State = JwtResultType.Success, Data = vuser };
        }

        /// <summary>
        /// 获取权限组
        /// </summary>
        /// <returns></returns>
        public async Task<List<Role>> GetRoles(Guid userid)
        {
            var db = _unitOfWork.GetDbClient();
            var rolesid = await db.Queryable<UserRole, User>((usr, us) => new object[]
            {
                JoinType.Left,usr.UserId==us.Id,
            }).Where(usr => usr.UserId == userid).Select(usr => usr.RoleId).FirstAsync();
            var roles = await db.Queryable<Role>().Where(x => x.Id == rolesid).ToListAsync();
            return roles;
        }
    }
}
