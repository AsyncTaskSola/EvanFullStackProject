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

            #region ��Ȩ 
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;//��jwttoken��cliam��ӳ����ر��ˣ����޸���Ȩ��������cliam
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
           .AddCookie("Cookies", option =>
           {
               option.AccessDeniedPath = "/Authorization/AccessDenied";
           })//�����Ȩ�ޣ�������ת
           .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
           {
               options.SignInScheme = "Cookies";
               options.Authority = "http://localhost:5001";
               options.RequireHttpsMetadata = false;
               options.ClientId = "hybrid client";
               options.ClientSecret = "hybrid secret";
               options.SaveTokens = true;
               options.ResponseType = "code id_token";//����Ҫ����Ȩ������Ͷ��˸�id_token

               #region Scope ��Χ
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

               #region ������Ķ��� ����Ҫ�����˵������ԣ�nbf amr exp...  ��Ҫ��ʱ���þ�����
               //options.ClaimActions.Remove("nbf");
               //options.ClaimActions.Remove("amr");
               //options.ClaimActions.Remove("exp");
               #endregion

               // ��ӳ�䵽User Claims��
               options.ClaimActions.DeleteClaim("sid");
               options.ClaimActions.DeleteClaim("sub");
               options.ClaimActions.DeleteClaim("idp");

               //��claim����Ľ�ɫ��Ϊmvcϵͳʶ��Ľ�ɫ  RBAC��ͳ����roles
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   NameClaimType = JwtClaimTypes.Name,    //��claimӳ�䵽mvcʶ���name,role
                   RoleClaimType = JwtClaimTypes.Role
               };
           });
            //services.AddAuthorization(options =>
            //{
            //    #region Ĭ�ϲ���
            //    options.AddPolicy("SmithInSomewhere", builder =>
            //   {
            //       builder.RequireAuthenticatedUser();//һ��Ҫ��½���û�
            //        builder.RequireClaim(JwtClaimTypes.FamilyName, "Smith");
            //       builder.RequireClaim("location", "somewhere");
            //   });
            //    #endregion
            //});

            #endregion
            #region ע�����
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
