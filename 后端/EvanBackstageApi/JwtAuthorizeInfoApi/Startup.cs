using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JwtAuthorizeInfoApi.GetJwtToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            services.AddAuthorization();
            #endregion

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
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
