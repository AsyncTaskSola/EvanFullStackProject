
using EvanBackstageApi.Entity.CEG;
using EvanBackstageApi.IRepository;
using EvanBackstageApi.IRepository.ICEGRepository;
using SqlSugar;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EvanBackstageApi.Repository.CEGRepository
{
    public class EmployeesRepository : BaseRepository<Employee>, IEmployeesRepository
    {
        protected readonly ISqlSugarClient _db;
        private readonly IUnitOfWork _unitOfWork;
        public EmployeesRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _db = unitOfWork.GetDbClient();
        }
        public Guid GetEcompanyInfo(Expression<Func<Employee, bool>> expression, Expression<Func<Employee, Guid>> expression2)
        {
           return _db.Queryable<Employee>().Where(expression).Select(expression2).First();
        }
    }
}

