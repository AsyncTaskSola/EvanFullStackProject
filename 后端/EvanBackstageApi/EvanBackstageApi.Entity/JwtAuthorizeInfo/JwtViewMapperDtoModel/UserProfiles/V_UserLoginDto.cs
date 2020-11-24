using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EvanBackstageApi.Entity.JwtAuthorizeInfo.JwtViewMapperDtoModel.UserProfiles
{
    public class V_UserLoginDto
    {
        /// <summary>
        /// 登陆账号
        /// </summary>
        //[Required(ErrorMessage = "请输入登陆账号")]
        //[MaxLength(16, ErrorMessage = "登陆账号最大长度是16")]
        //[MinLength(3, ErrorMessage = "登陆账号最小长度是3")]
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        //[Required(ErrorMessage = "请输入密码")]
        //[StringLength(32, ErrorMessage = "密码长度不对")]
        public string Password { get; set; }
    }
}
