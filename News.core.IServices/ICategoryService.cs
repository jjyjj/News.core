using News.core.Model;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace News.core.IServices
{

    public interface ICategoryService : IBaseService<Model.Entities.Category>
    {
        Task<List<Category>> GetAllCateUsers();
        Task<int> Add(string categoryName, int userId);
        Task<bool> Del(int CategoryId);
        Task<bool> Update(int categoryId, string categoryName);
    }

}
