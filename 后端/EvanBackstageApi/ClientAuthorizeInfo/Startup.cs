using System.IdentityModel.Tokens.Jwt;
using ClientAuthorizeInfo.Connected;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace ClientAuthorizeInfo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            #region 授权 
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;//把jwttoken的cliam的映射给关闭了，不修改授权服务器的cliam
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
           .AddCookie("Cookies", option =>
           {
               option.AccessDeniedPath = "/Authorization/AccessDenied";
           })//身份无权限，错误跳转
           .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
           {
               options.SignInScheme = "Cookies";
               options.Authority = "http://localhost:5001";
               options.RequireHttpsMetadata = false;
               options.ClientId = "hybrid client";
               options.ClientSecret = "hybrid secret";
               options.SaveTokens = true;
               options.ResponseType = "code id_token";//这里要比授权码的类型多了个id_token

               #region Scope 范围
               options.Scope.Clear();
               options.Scope.Add("api1");
               options.Scope.Add("openid");
               options.Scope.Add("profile");
               options.Scope.Add("email");
               options.Scope.Add("address");
               options.Scope.Add("phone");
               options.Scope.Add("roles");

               options.Scope.Add("locations");
               options.Scope.Add(OidcConstants.StandardScopes.OfflineAccess);
               #endregion

               #region 集合里的东西 都是要被过滤掉的属性，nbf amr exp...  需要的时候用就行了
               //options.ClaimActions.Remove("nbf");
               //options.ClaimActions.Remove("amr");
               //options.ClaimActions.Remove("exp");
               #endregion

               // 不映射到User Claims里
               options.ClaimActions.DeleteClaim("sid");
               options.ClaimActions.DeleteClaim("sub");
               options.ClaimActions.DeleteClaim("idp");

               //让claim里面的角色成为mvc系统识别的角色  RBAC传统类型roles
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   NameClaimType = JwtClaimTypes.Name,    //从claim映射到mvc识别的name,role
                   RoleClaimType = JwtClaimTypes.Role
               };
           });
            //services.AddAuthorization(options =>
            //{
            //    #region 默认策略
            //    options.AddPolicy("SmithInSomewhere", builder =>
            //   {
            //       builder.RequireAuthenticatedUser();//一定要登陆的用户
            //        builder.RequireClaim(JwtClaimTypes.FamilyName, "Smith");
            //       builder.RequireClaim("location", "somewhere");
            //   });
            //    #endregion
            //});

            #endregion
            #region 注入服务
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ILoginUserInfoRepository, LoginUserInfoRepository>();
            #endregion

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
