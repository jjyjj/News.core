using News.core.Model;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.core.IServices
{
    public interface IBaseService<T> where T : BaseEntity
    {

        Task<int> Create(T model);
        Task<bool> Delete(T model);
        Task<T> GetOneById(int id);
        Task<bool> Update(T model);
        Task<List<T>> GetAll();
      
    }
}
