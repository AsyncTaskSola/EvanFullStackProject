﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EvanBackstageApi.Entity.JwtAuthorizeInfo.JwtViewMapperDtoModel.RoleProfiles
{
    public  class V_RoleDto
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 权限描述
        /// </summary>
        public string Describe { get; set; }
    }
}
