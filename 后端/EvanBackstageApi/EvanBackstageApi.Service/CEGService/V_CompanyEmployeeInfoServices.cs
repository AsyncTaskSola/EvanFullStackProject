using EvanBackstageApi.Entity;
using EvanBackstageApi.Entity.View.V_CEG;
using EvanBackstageApi.IRepository;
using EvanBackstageApi.IRepository.ICEGRepository;
using EvanBackstageApi.IService.ICEGService;
using EvanBackstageApi.Repository.CEGRepository;
using System;
using System.Linq.Expressions;

namespace EvanBackstageApi.Service.CEGService
{
    public class V_CompanyEmployeeInfoServices : BaseService<V_CompanyEmployeeInfo>, IV_CompanyEmployeeInfoServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IV_CompanyEmployeeInfoRepository _dal;
        public V_CompanyEmployeeInfoServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dal = new V_CompanyEmployeeInfoRepository(unitOfWork);
            BaseDal = _dal;
        }
        public DataTableResult<V_CompanyEmployeeInfo> DataTable(Expression<Func<V_CompanyEmployeeInfo, bool>> whereLambda,
            int intPageIndex,
            int intPageSize,
            string strOrderByFileds,
            string Font,
            out int intTotalCount)
        {
            return _dal.DataTable(whereLambda, intPageIndex, intPageSize, strOrderByFileds, Font, out intTotalCount);
        }
    }
}
