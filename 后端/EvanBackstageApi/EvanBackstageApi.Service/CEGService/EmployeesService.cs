using EvanBackstageApi.Entity.CEG;
using EvanBackstageApi.Entity.UserInfo;
using EvanBackstageApi.IRepository;
using EvanBackstageApi.IRepository.ICEGRepository;
using EvanBackstageApi.IService.ICEGService;
using EvanBackstageApi.Repository.CEGRepository;

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
    }
}
