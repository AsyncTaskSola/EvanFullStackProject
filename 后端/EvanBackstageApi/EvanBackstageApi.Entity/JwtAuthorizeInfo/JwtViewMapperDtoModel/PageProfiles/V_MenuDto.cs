using System;
using System.Collections.Generic;
using System.Text;

namespace EvanBackstageApi.Entity.JwtAuthorizeInfo.JwtViewMapperDtoModel.PageProfiles
{
    public  class V_MenuDto
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string Module { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        public List<V_MenuDto> Children { get; set; }
    }
}
