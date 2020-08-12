using News.core.IRepository;
using News.core.IServices;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.core.Services
{
    public class BaseService<T> : IBaseService<T> where T :  BaseEntity,  new()
    {
        public IBaseRepository<T> BaseDal;
       

        public async Task<int> Create(T model)
        {
            return await BaseDal.Create(model);
        }

        public async Task<bool> Delete(T model)
        {
            return await BaseDal.Delete(model);
        }

        public IQueryable<T> GetAll()
        {
            return BaseDal.GetAll();
        }

        public async Task<T> GetOneById(int id)
        {
            return await BaseDal.GetOneById(id);
        }

        public async Task<bool> Update(T model)
        {
            return await BaseDal.Update(model);
        }
    }
}
