using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using SqlSugar;

namespace EvanBackstageApi.Entity.JwtAuthorizeInfo.JwtViewMapperDtoModel.UserProfiles
{
    public  class V_UserUpdateDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 登陆密码
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 32)]
        public string OldPassword { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 32)]
        public string NewPassword { get; set; }
        /// <summary>
        /// 头像文件
        /// </summary>
        public IFormFile Avatar { get; set; }
    }
}
