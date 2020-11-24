﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EvanBackstageApi.Entity.JwtAuthorizeInfo.JwtViewMapperDtoModel.UserProfiles
{
    public  class V_UsersViewDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 是否在线
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 用户权限组
        /// </summary>
        public List<string> Roles { get; set; }
    }
}
