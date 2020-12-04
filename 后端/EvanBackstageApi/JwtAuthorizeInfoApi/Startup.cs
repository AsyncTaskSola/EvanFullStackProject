using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using Autofac;
using AutoMapper;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Root;
using EvanBackstageApi.Extensions;
using EvanBackstageApi.IRepository.IJwtAuthorizeInfoRepository;
using EvanBackstageApi.Repository.JwtAuthorizeInfoRepository;
using JwtAuthorizeInfoApi.PolicyRequirment;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace JwtAuthorizeInfoApi
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
            services.AddControllers();
            #region swagger service
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var xmlFile = AppDomain.CurrentDomain.FriendlyName + ".xml";
            var xmlPath = Path.Combine(baseDirectory, xmlFile);
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("JwtAuthorizeInfoApi", new OpenApiInfo
                {
                    Version = "v2.0",
                    Title = "JwtAuthorizeInfoApi",
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
                option.IncludeXmlComments(xmlPath, true);

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

            #region 添加验证服务

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;//把jwttoken的cliam的映射给关闭了，不修改授权服务器的cliam
            services.AddAuthorization(options =>
            {
                #region 默认策略
                //options.AddPolicy("SmithInSomewhere", builder =>
                //{
                //    builder.RequireAuthenticatedUser();//一定要登陆的用户
                //    builder.RequireClaim(JwtClaimTypes.FamilyName, "Smith");
                //    builder.RequireClaim("location", "somewhere");
                //});
                #endregion
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,   // 发行人验证，这里要和token类中Claim类型的发行人保持一致
                            ValidateAudience = true, // 接收人验证
                            ValidateLifetime = true, //验证生命周期
                            ClockSkew = TimeSpan.FromSeconds(Convert.ToInt32(Configuration.GetSection("JWT")["ClockSkew"])),
                            ValidateIssuerSigningKey = true,// 是否开启签名认证
                            ValidAudience = Configuration.GetSection("JWT")["ValidAudience"],//订阅人
                            ValidIssuer = Configuration.GetSection("JWT")["ValidIssuer"],//发行人
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JWT")["IssuerSigningKey"]))
                        };
                    });

            services.AddAuthorization(options =>
            {
                var customizepolicyrequirment = new CustomizePolicyRequirment
                {
                    ClaimType = ClaimTypes.Role,//这个地方是重点啊,后面会不要用到
                    Expiration = TimeSpan.FromMinutes(60),//接口的过期时间
                    Roles = new List<Role>()
                };
                // 策略授权 1.基于角色
                //options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());//单独角色
                //options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                //options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));//或的关系
                //options.AddPolicy("SystemAndAdmin", policy => policy.RequireRole("Admin").RequireRole("System"));//且的关系
                //2.基于声明
                //options.AddPolicy("AdminClaim2", o => { o.RequireClaim("Role", "Admin", "User"); });

                //3.基于需要的Requirement  完全自定义 大多数都使用这个

                options.AddPolicy("CustomizePolicy", o => { o.Requirements.Add(customizepolicyrequirment); });
            });

            services.AddSingleton<IAuthorizationHandler, CustomizePolicyHandler>();
            #endregion

            #region automapper
            //var result = AppDomain.CurrentDomain.GetAssemblies();
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(EvanBackstageApi.Extensions.AutoMapper.UserProfile).Assembly);
            services.AddAutoMapper(typeof(EvanBackstageApi.Extensions.AutoMapper.RoleProfile).Assembly);
            services.AddAutoMapper(typeof(EvanBackstageApi.Extensions.AutoMapper.PageProfile).Assembly);
            #endregion

            #region 跨域
            services.AddCors(options =>
            {
                options.AddPolicy(name: "Policy1",
                    builder =>
                    {
                        builder.WithOrigins("http://127.0.0.1:8082", "http://localhost:8082")    //这里实际是前端写的接口
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            #endregion

            // HttpContextSetup
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();
        }
        #region autofac
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule());
        }
        #endregion
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(env.WebRootPath),
                RequestPath = new PathString("/src")
            });
            app.UseRouting();
            #region Cors
            app.UseCors("Policy1");
            #endregion
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/JwtAuthorizeInfoApi/swagger.json", "JwtAuthorizeInfoApi Docs");

                option.RoutePrefix = string.Empty;
                option.DocumentTitle = "EvanBackstage API by Author Evan";
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
