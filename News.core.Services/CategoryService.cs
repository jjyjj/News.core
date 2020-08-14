using News.core.IRepository;
using News.core.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Services
{

    public class CategoryService : BaseService<Model.Entities.Category>, ICategoryService
    {

        private readonly ICategoryRepostorycs _categoryRepostorycs;

        public CategoryService(ICategoryRepostorycs categoryRepostorycs)
        {
            _categoryRepostorycs = categoryRepostorycs ?? throw new ArgumentNullException(nameof(categoryRepostorycs));
            base.BaseDal = categoryRepostorycs;
        }

    }
}
