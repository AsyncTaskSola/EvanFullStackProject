using EvanBackstageApi.Entity.CEG;
using EvanBackstageApi.IRepository;
using EvanBackstageApi.IRepository.ICEGRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvanBackstageApi.Repository.CEGRepository
{
    public class CompaniesRepository : BaseRepository<Company>, ICompaniesRepository
    {
        public CompaniesRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
