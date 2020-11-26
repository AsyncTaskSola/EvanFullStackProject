using System;
using System.Collections.Generic;
using System.Text;

namespace EvanBackstageApi.Entity.JwtAuthorizeInfo.JwtViewMapperDtoModel.RoleProfiles
{
    public class V_RoleUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Describe { get; set; }
        public string Creator { get; set; }
        public bool IsDeleted { get; set; }
    }
}
