using EvanBackstageApi.Entity.CEG;
using EvanBackstageApi.Entity.UserInfo;
using EvanBackstageApi.IRepository;
using EvanBackstageApi.IRepository.ICEGRepository;
using EvanBackstageApi.IService.ICEGService;
using EvanBackstageApi.Repository.CEGRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvanBackstageApi.Service.CEGService
{
    public  class CompaniesService:BaseService<Company>, ICompaniesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICompaniesRepository _dal;
        public CompaniesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dal = new CompaniesRepository(unitOfWork);
            BaseDal = _dal;
        }
        public LoginUserInfo GetLoginInfo(string accessToken)
        {
           return  _unitOfWork.GetInfo(accessToken);
        }
    }
}
