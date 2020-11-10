using EvanBackstageApi.Entity;
using EvanBackstageApi.Entity.CEG;
using EvanBackstageApi.Entity.View.V_CEG;
using EvanBackstageApi.IRepository;
using EvanBackstageApi.IRepository.ICEGRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EvanBackstageApi.Repository.CEGRepository
{
    public class V_CompanyEmployeeInfoRepository : BaseRepository<V_CompanyEmployeeInfo>, IV_CompanyEmployeeInfoRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public V_CompanyEmployeeInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public DataTableResult<V_CompanyEmployeeInfo> DataTable(Expression<Func<V_CompanyEmployeeInfo, bool>> whereLambda,
          int intPageIndex,
          int intPageSize,
          string strOrderByFileds,
          string Font,
          out int intTotalCount)
        {
            intTotalCount = _db.Queryable<V_CompanyEmployeeInfo>().ToList().Count;

            var All = _db.Queryable<V_CompanyEmployeeInfo>().ToList();
            var result = new DataTableResult<V_CompanyEmployeeInfo>
            {
                Rows = intTotalCount,

                Data = _db.Queryable<V_CompanyEmployeeInfo>().Mapper(i =>
                {
                    i.Emplyees = _db.Queryable<Employee>().Where(x => x.CompanyId == i.Id).Mapper(o => o.CompanyName = _db.Queryable<Company>().Where(w => w.Id == i.Id).Select(p => p.Name).First()).ToList();
                })
                .OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                .WhereIF(whereLambda != null, whereLambda)
                .ToPageList(intPageIndex, intPageSize)
            };
            var data = result.Data as List<V_CompanyEmployeeInfo>;
            result.Data = data.Distinct(new Name()).ToList();
            result.Rows = data.Count;
            if (!string.IsNullOrEmpty(Font))
            {
                data = result.Data as List<V_CompanyEmployeeInfo>;
                //var d= data.Where(x => x.Name.Contains(Font) || x.FirstName.Contains(Font)).ToList();
                var d = data.Where(x => x.Name.Contains(Font)).ToList();
                result.Data = d;
                result.Rows =d.Count;
                return result;
            }
            return result;
        }
        public class Name : IEqualityComparer<V_CompanyEmployeeInfo>
        {
            public bool Equals(V_CompanyEmployeeInfo x, V_CompanyEmployeeInfo y)
            {
                if (x == null)
                    return y == null;
                return x.Name == y.Name;
            }

            public int GetHashCode(V_CompanyEmployeeInfo obj)
            {
                if (obj == null)
                    return 0;
                return obj.Name.GetHashCode();
            }
        }
    }
}
