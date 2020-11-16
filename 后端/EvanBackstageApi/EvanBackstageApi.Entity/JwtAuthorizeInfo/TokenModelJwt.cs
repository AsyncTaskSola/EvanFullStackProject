using System;
using System.Collections.Generic;
using System.Text;

namespace EvanBackstageApi.Entity.JwtAuthorizeInfo
{
    //令牌
    public class TokenModelJwt
    {

        /// <summary>
        /// Id
        /// </summary>
        public string Uid { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// 职能
        /// </summary>
        public string Work { get; set; }
    }
}
