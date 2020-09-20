using Microsoft.EntityFrameworkCore.Metadata.Internal;
using News.core.Model;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace News.core.IServices
{
    public interface IBaseService<T> where T : BaseEntity
    {
        void RollBack();
        Task<int> Create(T model);
        Task<bool> Delete(T model);
        Task<T> GetOneById(int id);
        Task<bool> Update(T model);
        Task<List<T>> GetAll();
       
        Task<PageModel<T>> GetAll(int pageIndex, int pageSize);


        Task<T> GetOneByStr(Expression<Func<T, bool>> strWhere);


        Task<List<T>> Query(Expression<Func<T, bool>> strWhere = null);
        /// <summary>
        /// 分页查询并排序
        /// </summary>
        /// <typeparam name="TEntity">返回的数据是什么</typeparam>
        /// <typeparam name="TKey">数据类型</typeparam>
        /// <param name="PageIndex">当前页面</param>
        /// <param name="PageSize">页面大小</param>
        /// <param name="OrderBy">按什么排序</param>
        /// <param name="WhereLambda">按什么条件查询，默认null</param>
        /// <param name="IsOrder">是否排序,默认true降序</param>
        /// <returns></returns>
        Task<PageModel<T>> Pagination<TEntity, TKey>(int PageIndex, int PageSize, Expression<Func<T, TKey>> OrderBy, Expression<Func<T, bool>> WhereLambda = null, bool isDesc = true);


    }
}
