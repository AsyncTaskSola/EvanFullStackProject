using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvanBackstageApi.Entity.JwtAuthorizeInfo.Root
{
    public  class UserRole
    {
        /// <summary>
        /// 自身Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid RoleId { get; set; }
    }
}
