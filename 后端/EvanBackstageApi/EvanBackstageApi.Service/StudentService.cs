using EvanBackstageApi.Entity;
using EvanBackstageApi.IRepository;
using EvanBackstageApi.IService;
using EvanBackstageApi.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvanBackstageApi.Service
{
    public class StudentService : BaseService<Student>, IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentRepository _dal;
        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dal = new StudentRepository(unitOfWork);
            BaseDal = _dal;
        }
    }
}
