using EvanBackstageApi.Entity;
using EvanBackstageApi.Entity.View.V_CEG;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EvanBackstageApi.IService.ICEGService
{
    public interface IV_CompanyEmployeeInfoServices : IBaseService<V_CompanyEmployeeInfo>
    {
        DataTableResult<V_CompanyEmployeeInfo> DataTable(Expression<Func<V_CompanyEmployeeInfo, bool>> whereLambda,
            int intPageIndex,
            int intPageSize,
            string strOrderByFileds,
            string Font,
            out int intTotalCount);
    }
}
