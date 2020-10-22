using EvanBackstageApi.IRepository;
using EvanBackstageApi.IService;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EvanBackstageApi.Service
{
    //写业务的一个延伸
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
    {
        public IBaseRepository<TEntity> BaseDal;

        public async Task<int> Add(TEntity model)
        {
            return await BaseDal.Add(model);
        }

        public async Task<int> Add(List<TEntity> listEntity)
        {
            return await BaseDal.Add(listEntity);
        }

        public async Task<int> Add(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null)
        {
            return await BaseDal.Add(entity, insertColumns);
        }

        public async Task<bool> Delete(TEntity entity)
        {
            return await BaseDal.Delete(entity);
        }

        public async Task<bool> DeleteById(object id)
        {
            return await BaseDal.DeleteById(id);
        }

        public async Task<bool> DeleteById(TEntity entity, Expression<Func<TEntity, bool>> expression)
        {
            return await BaseDal.DeleteById(entity, expression);
        }

        public async Task<bool> DeleteByIds(object[] ids)
        {
            return await BaseDal.DeleteByIds(ids);
        }

        public async Task<List<TEntity>> Query()
        {
            return await BaseDal.Query();
        }

        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await BaseDal.Query(whereExpression);
        }

        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return await BaseDal.Query(whereExpression, orderByExpression, isAsc);
        }

        public async Task<List<TEntity>> Query(string strWhere, string strOrderByFileds)
        {
            return await BaseDal.Query(strWhere, strOrderByFileds);
        }

        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intTop, string strOrderByFileds)
        {
            return await BaseDal.Query(whereExpression, intTop, strOrderByFileds);
        }

        public List<TEntity> Query(Expression<Func<TEntity, bool>> whereLambda, int intPageIndex, int intPageSize, string strOrderByFileds, out int intTotalCount)
        {
            return BaseDal.Query(whereLambda, intPageIndex, intPageSize, strOrderByFileds, out intTotalCount);
        }

        public async Task<bool> Update(TEntity entity)
        {
            return await BaseDal.Update(entity);
        }

        public async Task<bool> Update(List<TEntity> listEntities)
        {
            return await BaseDal.Update(listEntities);
        }

        public async Task<bool> Update(TEntity entity, Expression<Func<TEntity, object>> whereExpression)
        {
            return await BaseDal.Update(entity, whereExpression);
        }
        /// <summary>
        /// 单体约束
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity entity, Expression<Func<TEntity, bool>> whereExpression)
        {
            return await BaseDal.Update(entity, whereExpression);
        }
    }

}
