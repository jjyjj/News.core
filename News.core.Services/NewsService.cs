using Microsoft.EntityFrameworkCore;
using News.core.IRepository;
using News.core.IServices;
using News.core.Model;
using News.core.Model.Entities;
using News.core.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ICommentRepostory _commentRepostory;
        private readonly ICommentChildRepository _commentChildRepository;

        public NewsService(INewsRepository newsRepository, IUserRepository userRepository, INewsToCategoryRepository newsToCategoryRepository, ICategoryRepostorycs categoryRepostorycs, ICommentRepostory commentRepostory, ICommentChildRepository commentChildRepository)
        {
            _newsRepository = newsRepository ?? throw new ArgumentNullException(nameof(newsRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _newsToCategoryRepository = newsToCategoryRepository ?? throw new ArgumentNullException(nameof(newsToCategoryRepository));
            _categoryRepostorycs = categoryRepostorycs ?? throw new ArgumentNullException(nameof(categoryRepostorycs));
            _commentRepostory = commentRepostory ?? throw new ArgumentNullException(nameof(commentRepostory));
            _commentChildRepository = commentChildRepository ?? throw new ArgumentNullException(nameof(commentChildRepository));
            base.BaseDal = newsRepository;
        }

        public async Task<bool> delNew(int newsId)
        {
            var newsData = await _newsRepository.GetOneById(newsId);
            if (newsData == null) return false;
            else
            {
                try
                {
                    //先去删除newsTocategoryList关联表
                    var newsTocategoryList = (await _newsToCategoryRepository.GetAll()).FindAll(s => s.NewsId == newsId);
                    foreach (var item in newsTocategoryList)
                    {
                        await _newsToCategoryRepository.Delete(item);
                    }
                    //先删除子评论表
                    var _commentChildList = await _commentChildRepository.Query(m => m.NewsId == newsId);
                    foreach (var item in _commentChildList)
                    {
                        await _commentChildRepository.Delete(item);
                    }
                    //在删除评论表
                    var commentsList = await _commentRepostory.Query(m => m.NewsId == newsId);
                    foreach (var item in commentsList)
                    {
                        await _commentRepostory.Delete(item);
                    }
                    //在删除文章表
                    await _newsRepository.Delete(newsData);
                    return true;
                }
                catch (Exception)
                {

                    throw;
                }

            }

        }

        public async Task<PageModel<Model.Entities.News>> GetAllByCategory(string categoryName, QueryModel queryModel)
        {

            //获取当前类别
            var category = await _categoryRepostorycs.GetOneByStr(m => m.Name == categoryName);
            //根据类别id 查新闻id集合
            var newsToCategoryList = await _newsToCategoryRepository.Query(m => m.CategoryId == category.Id);

            List<Model.Entities.News> newsList = new List<Model.Entities.News>();
            PageModel<Model.Entities.News> pageModel = new PageModel<Model.Entities.News>();
            foreach (var item in newsToCategoryList)
            {
                var news = await _newsRepository.GetOneById(item.NewsId.Value);

                newsList.Add(news);
            }

            foreach (var items in newsList)
            {
                items.User = await _userRepository.GetOneById(items.UserId.Value);
            }


            pageModel.dataCount = newsList.Count;
            pageModel.pageCount = (int)Math.Ceiling(pageModel.dataCount / (double)queryModel.pageSize);
            pageModel.pageSize = queryModel.pageSize;
            pageModel.pageIndex = queryModel.pageIndex;
            var ss = newsList.AsQueryable();
            pageModel.data = ss
                .Skip((pageModel.pageIndex - 1) * pageModel.pageSize)
                .Take(pageModel.pageSize)
                .ToList() ?? null;

            return pageModel;
        }

        public async Task<PageModel<Model.ViewModel.NewsDetailsModel>> GetAllByUserId(int userId, QueryModel queryModel, int state)
        {

            var userData = await _userRepository.GetOneById(userId);
            PageModel<Model.ViewModel.NewsDetailsModel> pageModel = new PageModel<Model.ViewModel.NewsDetailsModel>();
            List<Model.ViewModel.NewsDetailsModel> NewsDetailsModel = new List<Model.ViewModel.NewsDetailsModel>();

            //获取到了用户新闻列表
            var newsList = await _newsRepository.Query(m => m.UserId == userData.Id && m.State == state);
            //循环去查找对应类别,并将news和类别装入NewsDetailsModel，最后放入list集合
            foreach (var item in newsList)
            {
                NewsDetailsModel newsDetailsModel = new NewsDetailsModel();
                var newsTocategoryList = await _newsToCategoryRepository.Query(m => m.NewsId == item.Id);
                foreach (var newsTocategory in newsTocategoryList)
                {
                    var categoryData = await _categoryRepostorycs.GetOneById(newsTocategory.CategoryId.Value);
                    newsDetailsModel.Categories.Add(categoryData);

                }
                newsDetailsModel.News = item;
                NewsDetailsModel.Add(newsDetailsModel);
            }

            pageModel.dataCount = NewsDetailsModel.Count;
            pageModel.pageCount = (int)Math.Ceiling(pageModel.dataCount / (double)queryModel.pageSize);
            pageModel.pageSize = queryModel.pageSize;
            pageModel.pageIndex = queryModel.pageIndex;
            var ss = NewsDetailsModel.AsQueryable();
            pageModel.data = ss
                .Skip((pageModel.pageIndex - 1) * pageModel.pageSize)
                .Take(pageModel.pageSize)
                .ToList() ?? null;

            return pageModel;

        }

        public async Task<Model.ViewModel.NewsDetailsModel> GetDetailsById(int newsId)
        {
            //获取该文章详情
            var news = await _newsRepository.GetOneById(newsId);
            //获取该文章的创建人信息
            news.User = await _userRepository.GetOneById(news.UserId.Value);
            //获取该文章下的所有类别
            var newsToCategoryData = await _newsToCategoryRepository.Query(m => m.NewsId == news.Id);
            NewsDetailsModel newsDetailsModel = new NewsDetailsModel();
            foreach (var item in newsToCategoryData)
            {
                var category = await _categoryRepostorycs.GetOneById(item.CategoryId.Value);
                newsDetailsModel.Categories.Add(category);
            }

            //获取上下一条文章id和标题
            Model.Entities.News previousNews;
            Model.Entities.News nextNews;

            var newsList = await _newsRepository.Query();
            int newsIndex = newsList.FindIndex(m => m.Id == newsId);
            if (newsIndex >= 0)
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
        /// <summary>
        /// 根据评论次数排序获取新闻数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public async Task<PageModel<HotCommentNewsViewModel>> HotCommentNews(QueryModel queryModel)
        {
            var newsList = await _newsRepository.GetAll();

            List<HotCommentNewsViewModel> hotCommentNewsViewModels = new List<HotCommentNewsViewModel>();
            foreach (var item in newsList)
            {
                HotCommentNewsViewModel hotCommentNewsViewModel = new HotCommentNewsViewModel();
                var count = (await _commentRepostory.Query(m => m.NewsId == item.Id)).Count();
                hotCommentNewsViewModel.News = item;
                hotCommentNewsViewModel.Id = item.Id;
                hotCommentNewsViewModel.CommentCount = count;
                hotCommentNewsViewModels.Add(hotCommentNewsViewModel);
            }

            #region MyRegion
            PageModel<HotCommentNewsViewModel> pageModel = new Model.PageModel<HotCommentNewsViewModel>();
            IQueryable<HotCommentNewsViewModel> queryable = hotCommentNewsViewModels.AsQueryable();
            //排序
            queryable = queryModel.isDesc == true ? queryable.OrderByDescending(m => m.CommentCount) : queryable.OrderBy(m => m.CommentCount);

            pageModel.dataCount = hotCommentNewsViewModels.Count;
            pageModel.pageCount = (int)Math.Ceiling(pageModel.dataCount / (double)queryModel.pageSize);
            pageModel.pageSize = queryModel.pageSize;
            pageModel.pageIndex = queryModel.pageIndex;

            pageModel.data = queryable.Skip((pageModel.pageIndex - 1) * queryModel.pageSize).Take(pageModel.pageSize).ToList();
            #endregion

            return pageModel;
        }

    }
}
