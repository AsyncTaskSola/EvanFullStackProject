using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using EvanBackstageApi.Basic;
using IdentityModel;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace EvanBackstageApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public static ILoggerRepository Repository { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

       
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers();
            services.AddControllersWithViews();
            #region 授权   不应该与资源者写在同一个地址上，注释分开写
           // JwtSecurityTokenHandler.DefaultMapInboundClaims = false;//把jwttoken的cliam的映射给关闭了，不修改授权服务器的cliam
           // services.AddAuthentication(options =>
           // {
           //     options.DefaultScheme = "Cookies";
           //     options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
           // })
           //.AddCookie("Cookies", option =>
           //{
           //    option.AccessDeniedPath = "/api/Authorization/AccessDenied";
           //})//身份无权限，错误跳转
           //.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
           //{
           //    options.SignInScheme = "Cookies";
           //    options.Authority = "http://localhost:5001";
           //    options.RequireHttpsMetadata = false;
           //    options.ClientId = "hybrid client";
           //    options.ClientSecret = "hybrid secret";
           //    options.SaveTokens = true;
           //    options.ResponseType = "code id_token";//这里要比授权码的类型多了个id_token

           //    //token 验证时间是1分钟
           //    options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(1);
           //    options.TokenValidationParameters.RequireExpirationTime = true;

           //    #region Scope 范围
           //    options.Scope.Clear();
           //    options.Scope.Add("api1");
           //    options.Scope.Add("openid");
           //    options.Scope.Add("profile");
           //    options.Scope.Add("email");
           //    options.Scope.Add("address");
           //    options.Scope.Add("phone");
           //    options.Scope.Add("roles");

           //    options.Scope.Add("locations");
           //    options.Scope.Add(IdentityModel.OidcConstants.StandardScopes.OfflineAccess);
           //     #endregion

           //     #region 集合里的东西 都是要被过滤掉的属性，nbf amr exp...  需要的时候用就行了
           //     //options.ClaimActions.Remove("nbf");
           //     //options.ClaimActions.Remove("amr");
           //     //options.ClaimActions.Remove("exp");
           //     #endregion

           //     // 不映射到User Claims里
           //    options.ClaimActions.DeleteClaim("sid");
           //    options.ClaimActions.DeleteClaim("sub");
           //    options.ClaimActions.DeleteClaim("idp");

           //     //让claim里面的角色成为mvc系统识别的角色  RBAC传统类型roles
           //    options.TokenValidationParameters = new TokenValidationParameters
           //    {
           //        NameClaimType = JwtClaimTypes.Name,    //从claim映射到mvc识别的name,role
           //         RoleClaimType = JwtClaimTypes.Role
           //    };
           //});
           // services.AddAuthorization(options =>
           // {
           //     #region 默认策略
           //     options.AddPolicy("SmithInSomewhere", builder =>
           //     {
           //         builder.RequireAuthenticatedUser();//一定要登陆的用户
           //         builder.RequireClaim(JwtClaimTypes.FamilyName, "Smith");
           //         builder.RequireClaim("location", "somewhere");
           //     });
           //     #endregion
           // });

            #endregion
            services.AddAuthentication("Bearer")
                 .AddJwtBearer("Bearer", options =>
                 {
                     options.Authority = "http://localhost:5001";
                     options.RequireHttpsMetadata = false;
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateAudience = false
                     };

                     //token 验证时间是30分钟
                     options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(30);
                     options.TokenValidationParameters.RequireExpirationTime = true;
                 });

            //注册接口和实现类的映射关系
            InjectionSetting.SetServices(ref services);
            #region 跨域
            services.AddCors(options =>
            {
                options.AddPolicy(name: "Policy1",
                    builder =>
                    {
                        builder.WithOrigins("http://127.0.0.1:8081", "http://localhost:8081")    //这里实际是前端写的接口
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            #endregion

            #region netcore log4 全局配置
            //var path = Directory.GetCurrentDirectory();

            Repository = LogManager.CreateRepository("AprilLog");
            XmlConfigurator.Configure(Repository, new FileInfo("Config/log4net.config"));//配置文件路径可以自定义
            BasicConfigurator.Configure(Repository);//控制台
            #endregion

            #region swagger service
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var xmlFile = AppDomain.CurrentDomain.FriendlyName + ".xml";
            var xmlPath = Path.Combine(baseDirectory, xmlFile);
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("CedenBackApi", new OpenApiInfo
                {
                    Version = "v2.0",
                    Title = "EvanBackstageApi",
                    Description = "API for EvanBackstage",
                    License = new OpenApiLicense
                    {
                        Name = "Git AsyncTask(Evan)",
                        Url = new Uri("https://github.com/AsyncTaskSola/EvanFullStackProject"),
                    },
                    Contact = new OpenApiContact
                    {
                        Name = "Evan Cnblogs",
                        Email = "22955559393@qq.com",
                        Url = new Uri("https://www.cnblogs.com/hexsola1314/p/"),
                    },
                });
                //加载XML注释文档
                // 第二参数includeControllerXmlComments 为true时控制器显示中文注释
                option.IncludeXmlComments(xmlPath, true);
                //可添加多份XML翻译档案 ，项目分布类所需要
                //option.IncludeXmlComments(Path.Combine(basePath, "DownLoadHaoKanVideoAPI.xml"), true);
                // include document file
                // option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(Startup).Assembly.GetName().Name}.xml"), true);


                //开启加权锁
                option.OperationFilter<AddResponseHeadersFilter>();
                option.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                //在header添加token，传到后台
                option.OperationFilter<SecurityRequirementsOperationFilter>();
                option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "jwt授权(输入方式Bearer {token}两者之间是一个空格)",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });


            });
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            #region Cors
            app.UseCors("Policy1");
            //app.UseCors(option =>
            //{
            //    option.WithOrigins("http://127.0.0.1:8081", "http://localhost:8081")
            //        .AllowAnyHeader()
            //        .AllowCredentials() //允许cookies
            //        .WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS");
            //});
            #endregion

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/CedenBackApi/swagger.json", "EvanBackstageApi Docs");

                option.RoutePrefix = string.Empty;
                option.DocumentTitle = "EvanBackstage API by Author Evan";
            });
            #endregion

            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseEvanMiddleware();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
