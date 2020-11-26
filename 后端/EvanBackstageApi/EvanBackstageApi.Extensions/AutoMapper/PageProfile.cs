using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.JwtViewMapperDtoModel.PageProfiles;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Root;

namespace EvanBackstageApi.Extensions.AutoMapper
{
    public class PageProfile: Profile
    {
        public PageProfile()
        {
            CreateMap<Menu, V_MenuDto>();
        }
    }
}
