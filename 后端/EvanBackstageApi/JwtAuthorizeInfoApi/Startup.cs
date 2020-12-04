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

                //������Ȩ��
                option.OperationFilter<AddResponseHeadersFilter>();
                option.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                //��header���token��������̨
                option.OperationFilter<SecurityRequirementsOperationFilter>();
                option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "jwt��Ȩ(���뷽ʽBearer {token}����֮����һ���ո�)",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });
            #endregion

            #region �����֤����

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;//��jwttoken��cliam��ӳ����ر��ˣ����޸���Ȩ��������cliam
            services.AddAuthorization(options =>
            {
                #region Ĭ�ϲ���
                //options.AddPolicy("SmithInSomewhere", builder =>
                //{
                //    builder.RequireAuthenticatedUser();//һ��Ҫ��½���û�
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
                            ValidateIssuer = true,   // ��������֤������Ҫ��token����Claim���͵ķ����˱���һ��
                            ValidateAudience = true, // ��������֤
                            ValidateLifetime = true, //��֤��������
                            ClockSkew = TimeSpan.FromSeconds(Convert.ToInt32(Configuration.GetSection("JWT")["ClockSkew"])),
                            ValidateIssuerSigningKey = true,// �Ƿ���ǩ����֤
                            ValidAudience = Configuration.GetSection("JWT")["ValidAudience"],//������
                            ValidIssuer = Configuration.GetSection("JWT")["ValidIssuer"],//������
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JWT")["IssuerSigningKey"]))
                        };
                    });

            services.AddAuthorization(options =>
            {
                var customizepolicyrequirment = new CustomizePolicyRequirment
                {
                    ClaimType = ClaimTypes.Role,//����ط����ص㰡,����᲻Ҫ�õ�
                    Expiration = TimeSpan.FromMinutes(60),//�ӿڵĹ���ʱ��
                    Roles = new List<Role>()
                };
                // ������Ȩ 1.���ڽ�ɫ
                //options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());//������ɫ
                //options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                //options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));//��Ĺ�ϵ
                //options.AddPolicy("SystemAndAdmin", policy => policy.RequireRole("Admin").RequireRole("System"));//�ҵĹ�ϵ
                //2.��������
                //options.AddPolicy("AdminClaim2", o => { o.RequireClaim("Role", "Admin", "User"); });

                //3.������Ҫ��Requirement  ��ȫ�Զ��� �������ʹ�����

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

            #region ����
            services.AddCors(options =>
            {
                options.AddPolicy(name: "Policy1",
                    builder =>
                    {
                        builder.WithOrigins("http://127.0.0.1:8082", "http://localhost:8082")    //����ʵ����ǰ��д�Ľӿ�
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
