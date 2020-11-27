using System;
using System.Collections.Generic;
using System.Text;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Root;

namespace EvanBackstageApi.IService.IJwtAuthorizeInfoService
{
    public  interface IMenuService: IBaseService<Menu>
    {
        List<Menu> FormatMenu(List<Menu> list);
        List<Menu> GAuthorMenus();
    }
}
