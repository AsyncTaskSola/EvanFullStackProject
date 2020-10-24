using EvanBackstageApi.Entity.CEG;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EvanBackstageApi.IRepository.ICEGRepository
{
    public interface IEmployeesRepository : IBaseRepository<Employee>
    {
        Guid GetEcompanyInfo(Expression<Func<Employee, bool>> expression, Expression<Func<Employee, Guid>> expression2);
    }
}
