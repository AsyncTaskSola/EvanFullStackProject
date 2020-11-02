// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Idp
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),

                new IdentityResource("roles","角色",new List<string>{ JwtClaimTypes.Role}),
                new IdentityResource("locations","地点",new List<string>{ "location"})
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                //new ApiScope("scope1"),
                //new ApiScope("scope2"),

                new ApiScope("api1",new List<string>{ "location"})//jwt            
            };
        #region reference token  超级安全，但很少用，所以jwt token是够用的
        //public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        //{
        //    //采用reference token 形式
        //    new ApiResource("api1",new List<string>{ "location" })
        //    {
        //        ApiSecrets={new Secret("api1 serret".Sha256())}
        //    }
        // };
        #endregion


        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                 #region 自动生产
                //  // m2m client credentials flow client
                //new Client
                //{
                //    ClientId = "m2m.client",
                //    ClientName = "Client Credentials Client",

                //    AllowedGrantTypes = GrantTypes.ClientCredentials, //最简单的 客户端和服务器做交互的，没有人
                //    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                //    AllowedScopes = { "scope1" }
                //},

                //// interactive client using code flow + pkce
                //new Client
                //{
                //    ClientId = "interactive",
                //    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                //    AllowedGrantTypes = GrantTypes.Code,

                //    RedirectUris = { "https://localhost:44300/signin-oidc" },
                //    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                //    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                //    AllowOfflineAccess = true,
                //    AllowedScopes = { "openid", "profile", "scope2" }
                //},
	             #endregion
                 
                //  ClientCredentials  不涉及到用户  靠的是Secret的客户凭证
                new Client
                {
                    ClientId = "Console client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials, //最简单的 平台和服务器做交互的，没有人
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },//凭证

                    AllowedScopes = { "api1"/*, IdentityServerConstants.StandardScopes.OpenId*/ } //不能访问openid没有人
                },

                //wpf client ,password grant 
                new Client
                {
                     ClientId = "Wpf client",
                     AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                     ClientSecrets = { new Secret("Wpf Secret".Sha256()) },//凭证
                     AllowedScopes = { "api1" , IdentityServerConstants.StandardScopes.OpenId , IdentityServerConstants.StandardScopes.Profile }
                },

                // mvc client, authorization code
                new Client
                {
                    ClientId = "mvc client",
                    ClientName = "ASP.NET Core MVC Client",

                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    ClientSecrets = { new Secret("mvc secret".Sha256()) },

                    RedirectUris = { "http://localhost:6002/signin-oidc" },

                    FrontChannelLogoutUri = "http://localhost:6002/signout-oidc",
                    PostLogoutRedirectUris = {"http://localhost:6002/signout-callback-oidc" },

                    AlwaysIncludeUserClaimsInIdToken = true,
                    
                    AllowOfflineAccess = true, // offline_access
                    AccessTokenLifetime = 60, // 60 seconds

                    AllowedScopes =
                    {
                        "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },

               //mvc hybrid Client
                new Client
                {
                    ClientId="hybrid client",
                    ClientName="Asp.net Core hybrid 客户端",
                    ClientSecrets = {new Secret("hybrid secret".Sha256())},//因为这里要进行客户端的认证
                    
                    RequirePkce = false,
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris =
                    {
                        "http://localhost:7000/signin-oidc"
                    },
                    //登出的uri
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:7000/signout-callback-oidc"
                    },

                    AllowOfflineAccess = true,  //使用refresh token
                    AccessTokenLifetime = 1200, // 1200 seconds
                    AlwaysIncludeUserClaimsInIdToken = true,//useclaim添加到id_token里面 个人喜好  要把jwt的cliam映射给关闭

                    //默认是jwt token  现在改成reference token
                    //AccessTokenType=AccessTokenType.Reference,

                    AllowedScopes =
                    {
                        "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone, 
                        //角色  rbac
                        "roles",
                        //策略 abac
                        "locations"
                    }
                }
            };
    }
}