using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EvanBackstageApi.IRepository;
using SqlSugar;

namespace EvanBackstageApi.Repository
{

    /// <summary>
    /// 作者:Evan 以下所有方法的方法均参考官方文档延伸
    /// 仓储层数据表操作 查 增加 删除 修改 获取 计算 合计 不做业务处理
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        protected readonly ISqlSugarClient _db;
        private readonly IUnitOfWork _unitOfWork;
        public BaseRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _db = unitOfWork.GetDbClient();
        }
        /// <summary>
        /// 添加单个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> Add(TEntity entity)
        {
            return await _db.Insertable(entity).ExecuteReturnIdentityAsync();
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="listEntity"></param>
        /// <returns></returns>
        public async Task<int> Add(List<TEntity> listEntity)
        {
            return await _db.Insertable(listEntity.ToArray()).ExecuteCommandAsync();
        }

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>返回自增量列</returns>
        public async Task<int> Add(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null)
        {
            var insert = _db.Insertable(entity);
            if (insertColumns == null)
            {
                return await insert.ExecuteReturnIdentityAsync();
            }
            else
            {
                return await insert.InsertColumns(insertColumns).ExecuteReturnIdentityAsync();
            }
        }

        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> Delete(TEntity entity)
        {
            return await _db.Deleteable(entity).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<bool> DeleteById(object id)
        {
            return await _db.Deleteable<TEntity>(id).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 根据删除条件删除
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Task<bool> DeleteById(TEntity entity, Expression<Func<TEntity, bool>> expression)
        {
            return _db.Deleteable(entity).Where(expression).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public async Task<bool> DeleteByIds(List<Guid> ids)
        {
            return await _db.Deleteable<TEntity>().In(ids).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 功能描述:查询所有数据
        /// </summary>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query()
        {
            return await _db.Queryable<TEntity>().ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询数据列表（条件约束）
        /// </summary>
        /// <param name="whereExpression">条件约束param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await _db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询一个列表（条件约束） 排序
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression">排序约束条件</param>
        /// <param name="isAsc">正序还是倒叙</param>
        /// <returns></returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression,
            Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return await _db.Queryable<TEntity>()
                .OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc)
                .WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(string strWhere, string strOrderByFileds)
        {
            return await _db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询前N条数据
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression,
            int intTop,
            string strOrderByFileds)
        {
            return await _db.Queryable<TEntity>()
                .OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                .WhereIF(whereExpression != null, whereExpression).Take(intTop).ToListAsync();
        }

        /// <summary>
        /// 更新实体（编辑）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity entity)
        {
            return await _db.Updateable<TEntity>(entity).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 批量更新（编辑）
        /// </summary>
        /// <param name="listEntities"></param>
        /// <returns></returns>
        public async Task<bool> Update(List<TEntity> listEntities)
        {
            return await _db.Updateable(listEntities).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 批量更新（新加约束）2020.11.02
        /// </summary>
        /// <param name="listEntities"></param>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public async Task<bool> Update(List<TEntity> listEntities, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, bool>> whereExpression2)
        {
            return await _db.Updateable(listEntities).Where(whereExpression).SetColumns(whereExpression2).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 更新约束，除开列 可以写C=>C.Name=="****"
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity entity, Expression<Func<TEntity, object>> whereExpression)
        {
            return await _db.Updateable(entity).IgnoreColumns(whereExpression).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 更新约束 更新条件 可以写C=>C.Name=="****"
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity entity, Expression<Func<TEntity, bool>> whereExpression)
        {
            return await _db.Updateable(entity).Where(whereExpression).ExecuteCommandHasChangeAsync();
        }





        /// <summary>
        /// 功能描述:分页查询 + 条件查询 + 排序
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="intTotalCount">数据总量</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public List<TEntity> Query(
            Expression<Func<TEntity, bool>> whereLambda,
            int intPageIndex,
            int intPageSize,
            string strOrderByFileds,
            out int intTotalCount)
        {
            intTotalCount = _db.Queryable<TEntity>().Where(whereLambda).Count();
            return _db.Queryable<TEntity>()
                .OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
                .WhereIF(whereLambda != null, whereLambda)
                .ToPageList(intPageIndex, intPageSize);
        }

        /// <summary>
        /// 删除实体并根据指定的条件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public int DeleteEntity(TEntity entity, Expression<Func<TEntity, bool>> whereLambda)
        {
            return _db.Deleteable(entity).Where(whereLambda).ExecuteCommand();
        }
        /// <summary>
        /// 更加条件查到实体，并且是第一条数据
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public async Task<TEntity>QueryFirst(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await _db.Queryable<TEntity>().Where(whereExpression).FirstAsync();
        }
    }
}
