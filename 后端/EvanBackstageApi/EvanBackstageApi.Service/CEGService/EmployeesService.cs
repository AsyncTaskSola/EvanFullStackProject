using EvanBackstageApi.Entity.CEG;
using EvanBackstageApi.Entity.UserInfo;
using EvanBackstageApi.IRepository;
using EvanBackstageApi.IRepository.ICEGRepository;
using EvanBackstageApi.IService.ICEGService;
using EvanBackstageApi.Repository.CEGRepository;
using System;
using System.Linq.Expressions;

namespace EvanBackstageApi.Service.CEGService
{
    public  class EmployeesService : BaseService<Employee>, IEmployeesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeesRepository _dal;
        public EmployeesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dal = new EmployeesRepository(unitOfWork);
            BaseDal = _dal;
        }
        public LoginUserInfo GetLoginInfo(string accessToken)
        {
            return _unitOfWork.GetInfo(accessToken);
        }
        public void CreateTable(bool Backup = false, int StringDefaultLength = 100, params Type[] types)
        {
            _unitOfWork.CreateTable(false,100,types);
        }
        public Guid GetEcompanyInfo(Expression<Func<Employee, bool>> expression, Expression<Func<Employee, Guid>> expression2)
        {
            return _dal.GetEcompanyInfo(expression, expression2);
        }
    }
}
