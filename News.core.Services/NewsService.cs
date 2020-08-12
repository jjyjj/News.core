using News.core.IRepository;
using News.core.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Services
{

    public class NewsService : BaseService<Model.Entities.News>, INewsService
    {
        private readonly INewsRepository _newsRepository;

        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository ?? throw new ArgumentNullException(nameof(newsRepository));
            base.BaseDal = newsRepository;
        }

    }
}
