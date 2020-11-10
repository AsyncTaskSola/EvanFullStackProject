using EvanBackstageApi.Entity;
using EvanBackstageApi.Entity.CEG;
using EvanBackstageApi.Entity.View.V_CEG;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EvanBackstageApi.IRepository.ICEGRepository
{
    public interface IV_CompanyEmployeeInfoRepository : IBaseRepository<V_CompanyEmployeeInfo>
    {
        DataTableResult<V_CompanyEmployeeInfo> DataTable(Expression<Func<V_CompanyEmployeeInfo, bool>> whereLambda,
         int intPageIndex,
         int intPageSize,
         string strOrderByFileds,
         string Font,
         out int intTotalCount);
    }
}
