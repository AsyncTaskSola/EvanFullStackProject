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
            #region ��Ȩ   ��Ӧ������Դ��д��ͬһ����ַ�ϣ�ע�ͷֿ�д
           // JwtSecurityTokenHandler.DefaultMapInboundClaims = false;//��jwttoken��cliam��ӳ����ر��ˣ����޸���Ȩ��������cliam
           // services.AddAuthentication(options =>
           // {
           //     options.DefaultScheme = "Cookies";
           //     options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
           // })
           //.AddCookie("Cookies", option =>
           //{
           //    option.AccessDeniedPath = "/api/Authorization/AccessDenied";
           //})//�����Ȩ�ޣ�������ת
           //.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
           //{
           //    options.SignInScheme = "Cookies";
           //    options.Authority = "http://localhost:5001";
           //    options.RequireHttpsMetadata = false;
           //    options.ClientId = "hybrid client";
           //    options.ClientSecret = "hybrid secret";
           //    options.SaveTokens = true;
           //    options.ResponseType = "code id_token";//����Ҫ����Ȩ������Ͷ��˸�id_token

           //    //token ��֤ʱ����1����
           //    options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(1);
           //    options.TokenValidationParameters.RequireExpirationTime = true;

           //    #region Scope ��Χ
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

           //     #region ������Ķ��� ����Ҫ�����˵������ԣ�nbf amr exp...  ��Ҫ��ʱ���þ�����
           //     //options.ClaimActions.Remove("nbf");
           //     //options.ClaimActions.Remove("amr");
           //     //options.ClaimActions.Remove("exp");
           //     #endregion

           //     // ��ӳ�䵽User Claims��
           //    options.ClaimActions.DeleteClaim("sid");
           //    options.ClaimActions.DeleteClaim("sub");
           //    options.ClaimActions.DeleteClaim("idp");

           //     //��claim����Ľ�ɫ��Ϊmvcϵͳʶ��Ľ�ɫ  RBAC��ͳ����roles
           //    options.TokenValidationParameters = new TokenValidationParameters
           //    {
           //        NameClaimType = JwtClaimTypes.Name,    //��claimӳ�䵽mvcʶ���name,role
           //         RoleClaimType = JwtClaimTypes.Role
           //    };
           //});
           // services.AddAuthorization(options =>
           // {
           //     #region Ĭ�ϲ���
           //     options.AddPolicy("SmithInSomewhere", builder =>
           //     {
           //         builder.RequireAuthenticatedUser();//һ��Ҫ��½���û�
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

                     //token ��֤ʱ����30����
                     options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(30);
                     options.TokenValidationParameters.RequireExpirationTime = true;
                 });

            //ע��ӿں�ʵ�����ӳ���ϵ
            InjectionSetting.SetServices(ref services);
            #region ����
            services.AddCors(options =>
            {
                options.AddPolicy(name: "Policy1",
                    builder =>
                    {
                        builder.WithOrigins("http://127.0.0.1:8081", "http://localhost:8081")    //����ʵ����ǰ��д�Ľӿ�
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            #endregion

            #region netcore log4 ȫ������
            //var path = Directory.GetCurrentDirectory();

            Repository = LogManager.CreateRepository("AprilLog");
            XmlConfigurator.Configure(Repository, new FileInfo("Config/log4net.config"));//�����ļ�·�������Զ���
            BasicConfigurator.Configure(Repository);//����̨
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
                //����XMLע���ĵ�
                // �ڶ�����includeControllerXmlComments Ϊtrueʱ��������ʾ����ע��
                option.IncludeXmlComments(xmlPath, true);
                //����Ӷ��XML���뵵�� ����Ŀ�ֲ�������Ҫ
                //option.IncludeXmlComments(Path.Combine(basePath, "DownLoadHaoKanVideoAPI.xml"), true);
                // include document file
                // option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(Startup).Assembly.GetName().Name}.xml"), true);


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
            //        .AllowCredentials() //����cookies
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
