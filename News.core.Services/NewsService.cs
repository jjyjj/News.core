using News.core.IRepository;
using News.core.IServices;
using News.core.Model;
using News.core.Model.Entities;
using News.core.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace News.core.Services
{

    public class NewsService : BaseService<Model.Entities.News>, INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IUserRepository _userRepository;
        private readonly INewsToCategoryRepository _newsToCategoryRepository;
        private readonly ICategoryRepostorycs _categoryRepostorycs;

        public NewsService(INewsRepository newsRepository, IUserRepository userRepository, INewsToCategoryRepository newsToCategoryRepository, ICategoryRepostorycs categoryRepostorycs)
        {
            _newsRepository = newsRepository ?? throw new ArgumentNullException(nameof(newsRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _newsToCategoryRepository = newsToCategoryRepository ?? throw new ArgumentNullException(nameof(newsToCategoryRepository));
            _categoryRepostorycs = categoryRepostorycs ?? throw new ArgumentNullException(nameof(categoryRepostorycs));
            base.BaseDal = newsRepository;
        }

        public async Task<PageModel<Model.Entities.News>> GetAllByCategory(string categoryName)
        {

            //获取当前类别
            var categoryList = await _categoryRepostorycs.GetAll();
            var category = categoryList.Find(m => m.Name == categoryName);
            //根据类别id 查新闻id集合
            var newsToCategory = await _newsToCategoryRepository.GetAll();
            var newsToCategoryList = newsToCategory.FindAll(m => m.CategoryId == category.Id);
            List<Model.Entities.News> newsList = new List<Model.Entities.News>();
            PageModel<Model.Entities.News> s = new PageModel<Model.Entities.News>();
            foreach (var item in newsToCategoryList)
            {
                var news = await _newsRepository.GetOneById(item.NewsId.Value);
                newsList.Add(news);
            }
            s.data = newsList;
            s.dataCount = newsList.Count;
           
            return s;
        }

        public async Task<Model.ViewModel.NewsDetailsModel> GetDetailsById(int newsId)
        {
            //获取该文章详情
            var news = await _newsRepository.GetOneById(newsId);
            //获取该文章的创建人信息
            news.User = await _userRepository.GetOneById(news.UserId.Value);
            //获取该文章下的所有类别
            var newsToCategoryData = await _newsToCategoryRepository.GetAll();//这里只去到了

            newsToCategoryData = newsToCategoryData.FindAll(n => n.NewsId == news.Id);
            NewsDetailsModel newsDetailsModel = new NewsDetailsModel();
            foreach (var item in newsToCategoryData)
            {
                var categoryName = (await _categoryRepostorycs.GetOneById(item.CategoryId.Value)).Name;
                newsDetailsModel.Categories.Add(categoryName);
            }

            //获取上下一条文章id和标题
            Model.Entities.News previousNews;
            Model.Entities.News nextNews;
            var newsList = await _newsRepository.GetAll();
            int newsIndex = newsList.FindIndex(m => m.Id == newsId);
            if (newsIndex > 0)
            {

                try
                {
                    previousNews = newsIndex > 0 ? newsList[newsIndex - 1] : null;
                    nextNews = newsIndex + 1 < newsList.Count ? newsList[newsIndex + 1] : null;
                    if (previousNews != null)
                    {
                        newsDetailsModel.previous = previousNews.Title;
                        newsDetailsModel.previousId = previousNews.Id;
                    }
                    if (nextNews != null)
                    {
                        newsDetailsModel.next = nextNews.Title;
                        newsDetailsModel.nextId = nextNews.Id;
                    }

                }
                catch (Exception)
                {

                    throw;
                }

            }
          
            newsDetailsModel.News = news;

            return newsDetailsModel;
        }

    }
}
