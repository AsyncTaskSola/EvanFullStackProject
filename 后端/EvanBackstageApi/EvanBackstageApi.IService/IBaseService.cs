using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EvanBackstageApi.IService
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        Task<int> Add(TEntity entity);
        Task<int> Add(List<TEntity> listEntity);
        Task<int> Add(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null);
        Task<bool> Delete(TEntity entity);
        Task<bool> DeleteById(object id);
        Task<bool> DeleteById(TEntity entity, Expression<Func<TEntity, bool>> expression);
        Task<bool> DeleteByIds(List<Guid> ids);
        Task<List<TEntity>> Query();
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression,
            Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);

        Task<List<TEntity>> Query(string strWhere, string strOrderByFileds);

        Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression,
            int intTop,
            string strOrderByFileds);

        Task<bool> Update(TEntity entity);
        Task<bool> Update(List<TEntity> listEntities);
        Task<bool> Update(TEntity entity, Expression<Func<TEntity, object>> whereExpression);
        Task<bool> Update(TEntity entity, Expression<Func<TEntity, bool>> whereExpression);

        List<TEntity> Query(
            Expression<Func<TEntity, bool>> whereLambda,
            int intPageIndex,
            int intPageSize,
            string strOrderByFileds,
            out int intTotalCount);
    }
}
