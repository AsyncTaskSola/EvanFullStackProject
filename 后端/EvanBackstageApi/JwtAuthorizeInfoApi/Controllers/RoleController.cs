using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Base;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.JwtViewMapperDtoModel.RoleProfiles;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Root;
using EvanBackstageApi.IRepository.IJwtAuthorizeInfoRepository;
using EvanBackstageApi.IService.IJwtAuthorizeInfoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthorizeInfoApi.Controllers
{
    [ApiController]
    [Area("UserInfo")]
    [Route("api/[area]/[controller]")]
    [Authorize("CustomizePolicy")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly IUser _user;
        private readonly IUserService _userService;

        public RoleController(IRoleService roleService,IMapper mapper,IUser user,IUserService userService)
        {
            _roleService = roleService;
            _mapper = mapper;
            _user = user;
            _userService = userService;
        }

        [HttpGet("GetRole")]
        [AllowAnonymous]
        public async Task<JwtResultModel<List<V_RoleDto>>> GetRole(int pageSize, int pageindex, string oderyFont)
        {
            int t = 0;
            Expression<Func<Role, bool>> exp = c => true;
            var rolelist = _roleService.Query(exp, pageindex, pageSize, oderyFont="CreateDate desc", out t).ToList();
            var roles = _mapper.Map<List<V_RoleDto>>(rolelist);
            return new JwtResultModel<List<V_RoleDto>>{ Message = "获取数据成功", State = JwtResultType.Success,Data = roles,Total = t};
        }

        [HttpPost("Add")]
        public async Task<JwtResultModel<Role>> Add(V_RoleDto vRole)
        {
            var userid = _user.Id;
            var createName = _userService.QueryFirst(x => x.Id == userid).Result.Name;
            var roles =await _userService.GetRoles(userid);
            if (roles.Select(x=>x.Name).Contains("管理员"))
            {
                vRole.Id=Guid.NewGuid();
                var vrp = _mapper.Map<Role>(vRole);
                vrp.Id = Guid.NewGuid();
                vrp.IsDeleted = false;
                vrp.IsDeleted = false;
                vrp.Creator = createName;
                await _roleService.Update(vrp);
            }
            return new JwtResultModel<Role> {Message = "该账号没有相对权限，请使用管理员（权限）账号",State= JwtResultType.Error};
        }
        /// <summary>
        /// 假删除更新对应字段状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        public async Task<JwtResultModel<Role>> Delete(Guid id)
        {
            var roles = await _roleService.QueryFirst(x => x.Id == id);
            roles.IsDeleted = true;
            await _roleService.Update(roles);
            return new JwtResultModel<Role> { Message = "(删除)成功", State = JwtResultType.Success };
        }

        [HttpPost("Update")]
        public async Task<JwtResultModel<Role>> Update(V_RoleUpdateDto vRoleUpdate)
        {
            var role = _mapper.Map<Role>(vRoleUpdate);
            await _roleService.Update(role);
            return new JwtResultModel<Role> { Message = "更新成功", State = JwtResultType.Success };
        }
    }
}
