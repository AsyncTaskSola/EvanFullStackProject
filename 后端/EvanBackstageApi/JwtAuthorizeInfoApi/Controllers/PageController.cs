using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Base;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.JwtViewMapperDtoModel.PageProfiles;
using EvanBackstageApi.IRepository.IJwtAuthorizeInfoRepository;
using EvanBackstageApi.IService.IJwtAuthorizeInfoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthorizeInfoApi.Controllers
{
    /// <summary>
    /// 页面控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("CustomizePolicy")]
    public class PageController: ControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly IUser _user;
        private readonly IMapper _mapper;

        public PageController(IMenuService menuService,IUser user,IMapper mapper)
        {
            _menuService = menuService;
            _user = user;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMenu")]
        public async Task<JwtResultModel<List<V_MenuDto>>> GetMenu()
        {
            var isAdmin = _user.GetClaimValueByType(ClaimTypes.Role).Any(x => x == "Admin");
            if (isAdmin)
            {
                //管理员应该全部功能显示
                var menu = await _menuService.Query(x =>!x.IsDeleted);
                var result= _menuService.FormatMenu(menu);
                var data= _mapper.Map<List<V_MenuDto>>(result);
                return new JwtResultModel<List<V_MenuDto>> { Message = "查询成功", State = JwtResultType.Success,Data = data};
            }
            else
            {
                // 不需要权限的菜单
                var menus = await _menuService.Query(m => !m.IsAuth && !m.IsDeleted);
                //需要权限的菜单 
                var authmenus = _menuService.GAuthorMenus();
                menus.AddRange(authmenus);
                menus = _menuService.FormatMenu(menus);
                var arrangemenus = _mapper.Map<List<V_MenuDto>>(menus);
                return new JwtResultModel<List<V_MenuDto>> { Message = "查询成功", State = JwtResultType.Success, Data = arrangemenus };
            }
        }
    }
}
