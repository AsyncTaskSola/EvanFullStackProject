using EvanBackstageApi.IRepository;
using EvanBackstageApi.IRepository.ICEGRepository;
using EvanBackstageApi.IService;
using EvanBackstageApi.IService.ICEGService;
using EvanBackstageApi.Repository;
using EvanBackstageApi.Repository.CEGRepository;
using EvanBackstageApi.Service;
using EvanBackstageApi.Service.CEGService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        }
    }
}
