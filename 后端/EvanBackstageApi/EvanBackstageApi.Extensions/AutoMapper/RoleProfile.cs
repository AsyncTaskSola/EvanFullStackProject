using AutoMapper;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.JwtViewMapperDtoModel.RoleProfiles;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Root;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvanBackstageApi.Extensions.AutoMapper
{
    public  class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, V_RoleDto>();
        }
    }
}
