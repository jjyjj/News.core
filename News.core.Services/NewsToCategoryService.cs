using News.core.IRepository;
using News.core.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Services
{

    public class NewsToCategoryService : BaseService<Model.Entities.NewsToCategory>, INewsToCategoryService
    {

        private readonly INewsToCategoryRepository _newsToCategoryRepository;

        public NewsToCategoryService(INewsToCategoryRepository newsToCategoryRepository)
        {

            _newsToCategoryRepository = newsToCategoryRepository ?? throw new ArgumentNullException(nameof(newsToCategoryRepository));
            base.BaseDal = newsToCategoryRepository;
        }

    }
}
