
using EvanBackstageApi.Entity.CEG;
using EvanBackstageApi.IRepository;
using EvanBackstageApi.IRepository.ICEGRepository;

namespace EvanBackstageApi.Repository.CEGRepository
{
    public class EmployeesRepository : BaseRepository<Employee>, IEmployeesRepository
    {
        public EmployeesRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}

