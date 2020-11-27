using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using EvanBackstageApi.IRepository;
using EvanBackstageApi.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JwtAuthorizeInfoApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var IConfiguration = services.GetRequiredService<IConfiguration>();
                UnitOfWork unitOfWork = new UnitOfWork(IConfiguration);
                // ��ʼ�����ݿ�ͽ���
               // await unitOfWork.Init();
            }

            await host.RunAsync();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //  ��Ĭ�ϵ�ServiceProviderFactoryָ��ΪAutofacServiceProviderFactory
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
