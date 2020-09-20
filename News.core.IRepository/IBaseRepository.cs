using Microsoft.EntityFrameworkCore.Metadata.Internal;
using News.core.Model;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace News.core.IRepository
{
    public interface IBaseRepository<T> where T : BaseEntity, new()
    {
        void RollBack();

        Task<int> Create(T model);
        Task<bool> Delete(T model);
        Task<bool> Update(T model);
        Task<T> GetOneById(int id);
        Task<List<T>> GetAll();
        //Task<List<T>> GetNum(int num, Expression<Func<T, bool>> strWhere,);

        Task<PageModel<T>> GetAll(int pageIndex, int pageSize);


        #region 其他
        Task<T> GetOneByStr(Expression<Func<T, bool>> strWhere);



        Task<List<T>> Query(Expression<Func<T, bool>> strWhere = null);



        Task<PageModel<T>> Pagination<TEntity, TKey>(int PageIndex, int PageSize, Expression<Func<T, TKey>> OrderBy, Expression<Func<T, bool>> WhereLambda = null, bool isDesc = true);


        #endregion

    }
}
