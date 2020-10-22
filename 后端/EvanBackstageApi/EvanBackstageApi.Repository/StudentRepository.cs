using EvanBackstageApi.Entity;
using EvanBackstageApi.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvanBackstageApi.Repository
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
