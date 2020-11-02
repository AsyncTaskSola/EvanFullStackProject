using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EvanBackstageApi.Entity.UserInfo
{
    [SugarTable("LoginUserInfo")]
    public class LoginUserInfo
    {
        [SugarColumn(IsPrimaryKey = true)]
        public Guid ID { get; set; }
        //获取登陆信息(仅管理员查看)
        public string PrivacyUrl { get; set; }
        public bool IsAuthenticated { get; set; }
        public string AuthenticationType { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        //[Column(TypeName = "text")]
        public string AccessToken { get; set; }
        public DateTime DateTimeStart { get; set; }

        public string RefreshToken { get; set; }

        [SugarColumn(IsIgnore = true), NotMapped]
        //最近使用的当前时间
        public DateTime CurrentTime { get; set; }

        [SugarColumn(IsIgnore = true), NotMapped]
        //accessToken有效时间
        public int EffectiveTime => 30;
    }
}
