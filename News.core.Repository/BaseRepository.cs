using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using News.core.IRepository;
using News.core.Model;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace News.core.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
        NewsDbContext _db;
        DbSet<T> _dbSet;
        public BaseRepository(NewsDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _dbSet = db.Set<T>();
        }

        public async Task<int> Create(T model)
        {
            _dbSet.Add(model);

            //  _db.Entry<T>(model).State = EntityState.Added;
            await _db.SaveChangesAsync();

            return model.Id;

        }

        public async Task<bool> Delete(T model)
        {
            _dbSet.Remove(model);
            return await _db.SaveChangesAsync() > 0;
        }


        public async Task<List<T>> GetAll()
        {

            return await _dbSet.OrderByDescending(s => s.CreateTime).AsNoTracking().ToListAsync();
        }




        public async Task<T> GetOneById(int id)
        {

            return await _dbSet.FindAsync(id); ;
        }

        public async Task<PageModel<T>> GetAll(int pageIndex, int pageSize)
        {

            //数据总条数
            var newsLisy = _dbSet.OrderByDescending(s => s.CreateTime).AsNoTracking();
            var dataCount = await newsLisy.CountAsync();
            //数据
            var items = await newsLisy.Skip(
               (pageIndex - 1) * pageSize)
               .Take(pageSize).ToListAsync();
            //总页数
            var pageCount = (int)Math.Ceiling(dataCount / (double)pageSize);


            return new PageModel<T>()
            {
                data = items,
                dataCount = dataCount,
                pageIndex = pageIndex,
                pageSize = pageSize,
                pageCount = pageCount
            };



        }


        public async Task<bool> Update(T model)
        {
            _dbSet.Update(model);
            return await _db.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="strWhere">默认条件为空</param>
        /// <returns></returns>
        public async Task<List<T>> Query(Expression<Func<T, bool>> strWhere = null)
        {
            var data = _dbSet.OrderByDescending(s => s.CreateTime).AsNoTracking();
            return strWhere == null ? await data.ToListAsync() : await data.Where(strWhere).ToListAsync();

        }

        public async Task<PageModel<T>> Pagination<TEntity, TKey>(int PageIndex, int PageSize, Expression<Func<T, TKey>> OrderBy, Expression<Func<T, bool>> WhereLambda = null, bool isDesc = true)
        {
            IQueryable<T> QueryList = isDesc == true ? _dbSet.OrderByDescending(OrderBy) : _dbSet.OrderBy(OrderBy);

            if (WhereLambda != null)
            {
                QueryList = QueryList.Where(WhereLambda);
            }
            int totalCount = QueryList.Count();
            int pageCount = (int)Math.Ceiling(totalCount / (double)PageSize);
            var data = await QueryList.Skip(PageSize * (PageIndex - 1)).Take(PageSize).ToListAsync() ?? null;
            return new Model.PageModel<T>()
            {
                data = data,
                pageIndex = PageIndex,
                pageSize = PageSize,
                dataCount = totalCount,
                pageCount = pageCount
            };
        }

        public async Task<T> GetOneByStr(Expression<Func<T, bool>> strWhere)
        {
            return await _dbSet.Where(strWhere).FirstOrDefaultAsync(); ;
        }

        public void RollBack()
        {
            using (var tran = _db.Database.BeginTransaction())
            {
                tran.Rollback();
            }
        }
    }
}
