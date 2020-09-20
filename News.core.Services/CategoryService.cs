using News.core.IRepository;
using News.core.IServices;
using News.core.Model;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace News.core.Services
{

    public class CategoryService : BaseService<Model.Entities.Category>, ICategoryService
    {

        private readonly ICategoryRepostorycs _categoryRepostorycs;
        private readonly IUserRepository _userRepository;
        private readonly INewsToCategoryRepository _newsToCategoryRepository;

        public CategoryService(ICategoryRepostorycs categoryRepostorycs, IUserRepository userRepository, INewsToCategoryRepository newsToCategoryRepository)
        {
            _categoryRepostorycs = categoryRepostorycs ?? throw new ArgumentNullException(nameof(categoryRepostorycs));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _newsToCategoryRepository = newsToCategoryRepository ?? throw new ArgumentNullException(nameof(newsToCategoryRepository));
            base.BaseDal = categoryRepostorycs;
        }

        public async Task<int> Add(string categoryName, int userId)
        {
            var cate = await _categoryRepostorycs.GetOneByStr(m => m.Name == categoryName);
            if (cate == null)
            {
                return await _categoryRepostorycs.Create(new Category()
                {
                    Name = categoryName,
                    UserId = userId
                });
            }
            return -1;



        }

        public async Task<bool> Del(int CategoryId)
        {
            bool isDel = false;
            //查出是否存在该类别
            var cate = await _categoryRepostorycs.GetOneByStr(m => m.Id == CategoryId);
            //查是否有文章引用该类别
            var newsToCategory = await _newsToCategoryRepository.
                GetOneByStr(m => m.CategoryId == CategoryId);
            if (newsToCategory != null)
            {
                isDel = await _categoryRepostorycs.Delete(cate);
            }

            return isDel;


        }

        public async Task<bool> Update(int categoryId, string categoryName)
        {
            bool isUpdate = false;

            var cate = await _categoryRepostorycs.GetOneByStr(m => m.Id == categoryId);

            if (cate == null) return isUpdate;
            else
            {
                cate.Name = categoryName;
                isUpdate = await _categoryRepostorycs.Update(cate);

            }
            return isUpdate;
        }
    }
}
