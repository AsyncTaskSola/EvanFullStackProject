using EvanBackstageApi.Entity.View.V_CEG;
using EvanBackstageApi.IRepository;
using EvanBackstageApi.IRepository.ICEGRepository;
using EvanBackstageApi.IService.ICEGService;
using EvanBackstageApi.Repository.CEGRepository;

namespace EvanBackstageApi.Service.CEGService
{
    public  class V_CompanyEmployeeInfoServices : BaseService<V_CompanyEmployeeInfo>, IV_CompanyEmployeeInfoServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IV_CompanyEmployeeInfoRepository _dal;
        public V_CompanyEmployeeInfoServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dal = new V_CompanyEmployeeInfoRepository(unitOfWork);
            BaseDal = _dal;
        }
    }
}
