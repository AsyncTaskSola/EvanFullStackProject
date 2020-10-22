using EvanBackstageApi.Entity.View.V_CEG;
using EvanBackstageApi.IRepository;
using EvanBackstageApi.IRepository.ICEGRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvanBackstageApi.Repository.CEGRepository
{
    public class V_CompanyEmployeeInfoRepository : BaseRepository<V_CompanyEmployeeInfo>, IV_CompanyEmployeeInfoRepository
    {
        public V_CompanyEmployeeInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
