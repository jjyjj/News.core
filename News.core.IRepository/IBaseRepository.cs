using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.core.IRepository
{
    public interface IBaseRepository<T> where T : BaseEntity, new()
    {
        Task<int> Create(T model);
        Task<bool> Delete(T model);
        Task<bool> Update(T model);
        Task<T> GetOneById(int id);
        IQueryable<T> GetAll();

    }
}
