using Microsoft.EntityFrameworkCore;
using News.core.IRepository;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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

            _db.Entry<T>(model).State = EntityState.Added;
            //var data = _dbSet.Add(model);
            //return await _db.SaveChangesAsync();
            // var data = _db.Set<T>().Add(model);
            return await _db.SaveChangesAsync();


        }

        public async Task<bool> Delete(T model)
        {
            _dbSet.Remove(model);
            return await _db.SaveChangesAsync() > 0;
        }

        public IQueryable<T> GetAll()
        {

            return _dbSet.AsNoTracking();
        }

        public async Task<T> GetOneById(int id)
        {
            var s = await GetAll().FirstAsync(m => m.Id == id);
            return s;
        }

        public async Task<bool> Update(T model)
        {
            _db.Entry(model).State = EntityState.Modified;
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
