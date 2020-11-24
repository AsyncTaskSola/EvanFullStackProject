using Autofac;
using EvanBackstageApi.IRepository;
using EvanBackstageApi.IService;
using EvanBackstageApi.Repository;
using EvanBackstageApi.Service;
using System.Reflection;
using Module = Autofac.Module;

namespace EvanBackstageApi.Extensions
{
    //https://autofaccn.readthedocs.io/zh/latest/register/registration.html#id10
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // 注册泛型仓储
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency();

            // 注册泛型服务
            builder.RegisterGeneric(typeof(BaseService<>)).As(typeof(IBaseService<>)).InstancePerDependency();

            //  注册Repository
            var assemblysRepository = Assembly.Load("EvanBackstageApi.Repository");
            builder.RegisterAssemblyTypes(assemblysRepository)
                .InstancePerDependency() // 每次调用，都会重新实例化对象；每次请求都创建一个新的对象； = AddScope
                .AsImplementedInterfaces();

            //  注册Services
            var assemblysServices = Assembly.Load("EvanBackstageApi.Service");
            builder.RegisterAssemblyTypes(assemblysServices)
                .InstancePerDependency()
                .AsImplementedInterfaces();
        }
    }
}
