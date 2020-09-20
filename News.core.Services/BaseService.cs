using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using News.core.IRepository;
using News.core.IServices;
using News.core.Model;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace News.core.Services
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity, new()
    {
        public IBaseRepository<T> BaseDal;


        public async Task<int> Create(T model)
        {
            var s = await BaseDal.Create(model) > 0;
            if (s)
            {
                return model.Id;
            }
            else
            {
                return 0;
            }

        }

        public async Task<bool> Delete(T model)
        {
            return await BaseDal.Delete(model);
        }

        public async Task<T> GetOneById(int id)
        {
            return await BaseDal.GetOneById(id);
        }

        public async Task<bool> Update(T model)
        {

            return await BaseDal.Update(model);
        }
        public async Task<List<T>> GetAll()
        {
            return await BaseDal.GetAll();
        }


        public async Task<PageModel<T>> GetAll(int pageIndex, int pageSize)
        {
            return await BaseDal.GetAll(pageIndex, pageSize);
        }



        public async Task<List<T>> Query(Expression<Func<T, bool>> strWhere = null)
        {
            return await BaseDal.Query(strWhere);
        }

        public async Task<PageModel<T>> Pagination<TEntity, TKey>(int PageIndex, int PageSize, Expression<Func<T, TKey>> OrderBy, Expression<Func<T, bool>> WhereLambda = null, bool isDesc = true)
        {
            return await BaseDal.Pagination<TEntity, TKey>(PageIndex, PageSize, OrderBy, WhereLambda, isDesc);
        }

        public async Task<T> GetOneByStr(Expression<Func<T, bool>> strWhere)
        {
            return await BaseDal.GetOneByStr(strWhere);
        }

        public void RollBack()
        {
            BaseDal.RollBack();
            return;
        }
    }
}
