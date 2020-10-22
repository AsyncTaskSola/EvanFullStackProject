using EvanBackstageApi.IRepository;
using EvanBackstageApi.IRepository.ICEGRepository;
using EvanBackstageApi.IService;
using EvanBackstageApi.IService.ICEGService;
using EvanBackstageApi.Repository;
using EvanBackstageApi.Repository.CEGRepository;
using EvanBackstageApi.Service;
using EvanBackstageApi.Service.CEGService;
using Microsoft.Extensions.DependencyInjection;

namespace EvanBackstageApi.Basic
{
    public class InjectionSetting
    {
        public static void SetServices(ref IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IStudentRepository, StudentRepository>();

            services.AddScoped<ICompaniesService, CompaniesService>();
            services.AddScoped<ICompaniesRepository, CompaniesRepository>();

            services.AddScoped<IEmployeesService, EmployeesService>();
            services.AddScoped<IEmployeesRepository, EmployeesRepository>();

            services.AddScoped<IV_CompanyEmployeeInfoServices, V_CompanyEmployeeInfoServices>();
            services.AddScoped<IV_CompanyEmployeeInfoRepository, V_CompanyEmployeeInfoRepository>();
           
        }
    }
}
