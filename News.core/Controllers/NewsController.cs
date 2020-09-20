using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News.core.IServices;
using News.core.Model;
using News.core.Model.Entities;
using News.core.Model.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;

namespace News.core.Controllers
{
    public class NewsController : BaseController
    {
        private readonly INewsService _newsService;
        private readonly IUserService _userService;
        private readonly INewsToCategoryService _newsToCategoryService;
        private readonly ICategoryService _categoryService;

        public NewsController(INewsService newsService, IUserService userService, INewsToCategoryService newsToCategoryService, ICategoryService categoryService)
        {
            _newsService = newsService ?? throw new ArgumentNullException(nameof(newsService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _newsToCategoryService = newsToCategoryService ?? throw new ArgumentNullException(nameof(newsToCategoryService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }
        #region 添加文章
        [HttpPost]
        public async Task<MessageModel> Add([FromBody] AddArticleModel newsViewModel, int state)
        {
            MessageModel messageModel = new MessageModel();
            var user = await _userService.GetOneById(newsViewModel.UserId);
            if (user == null) messageModel.Msg = "用户不存在";
            else
            {
                Model.Entities.News news = new Model.Entities.News();

                news.Title = newsViewModel.Title;
                news.UserId = newsViewModel.UserId;
                news.Content = newsViewModel.Content;
                news.State = state;
                news.ImagePath = newsViewModel.ImagePath;


                var newsId = await _newsService.Create(news);

                if (newsId < 0) messageModel.Msg = "创建失败";
                else
                {
                    try
                    {
                        foreach (var item in newsViewModel.categories)
                        {
                            NewsToCategory newsToCategory = new NewsToCategory();
                            newsToCategory.CategoryId = item;
                            newsToCategory.NewsId = newsId;
                            await _newsToCategoryService.Create(newsToCategory);
                        }
                        messageModel.Msg = "创建新闻成功";
                    }
                    catch (Exception)
                    {
                        var data = await _newsService.GetOneById(newsId);
                        await _newsService.Delete(data);
                        messageModel.Msg = "创建失败";
                    }
                }
                messageModel.Data = newsId;
            }
            messageModel.Code = 200;

            return messageModel;
        }
        #endregion

        #region 获取文章列表
        [HttpGet]
        public async Task<MessageModel> GetAll(QueryModel queryModel, int state)
        {
            MessageModel messageModel = new MessageModel();

            try
            {
                if (!queryModel.isPage) messageModel.Data = await _newsService.Query(m => m.State == 0);
                else
                {
                    var newsList = await _newsService.Pagination<Model.Entities.News, object>(queryModel.pageIndex, queryModel.pageSize, n => n.CreateTime, x => x.State == state, queryModel.isDesc);
                    foreach (var item in newsList.data)
                    {
                        item.User = await _userService.GetOneById(item.UserId.Value);
                    }
                    messageModel.Data = newsList;
                }
                messageModel.Code = 200;
                messageModel.Msg = "获取数据成功";
            }
            catch (Exception)
            {

                throw;
            }
            return messageModel;


        }
        #endregion

        #region 获取文章详情
        [HttpGet]
        public async Task<MessageModel> GetDetailsById(int newsId)
        {
            MessageModel messageModel = new MessageModel();
            try
            {
                var newsData = await _newsService.GetOneById(newsId);
                if (newsData == null) messageModel.Msg = "不存在该文章";
                else
                {
                    try
                    {
                        messageModel.Data = await _newsService.GetDetailsById(newsId);
                        await Browse(newsId);
                        messageModel.Msg = "获取文章详情成功";
                    }
                    catch (Exception)
                    {

                        throw;
                    }


                }
                messageModel.Code = 200;
            }
            catch (Exception)
            {

                throw;
            }
            return messageModel;

        }
        #endregion

        #region 根据类别获取文章列表
        [HttpGet]
        public async Task<MessageModel> GetAllByCategory(string categoryName, QueryModel queryModel)
        {
            MessageModel messageModel = new MessageModel();
            try
            {
                var categoryDate = await _categoryService.GetOneByStr(m => m.Name == categoryName);
                if (categoryDate == null) messageModel.Msg = "该类别不存在";
                else
                {
                    messageModel.Data = await _newsService.GetAllByCategory(categoryName, queryModel);
                    messageModel.Msg = "获取成功";
                }
                messageModel.Code = 200;


            }
            catch (Exception)
            {

                throw;
            }
            return messageModel;
        }
        #endregion

        #region 删除文章
        [HttpDelete]
        public async Task<MessageModel> Del(int newsId)
        {
            var messageModel = new MessageModel();
            if (newsId <= 0) messageModel.Msg = "文章不存在";
            else
            {
                if (await _newsService.delNew(newsId)) messageModel.Msg = "删除成功";


            }
            messageModel.Code = 200;
            return messageModel;

        }
        #endregion


        #region 修改文章
        [HttpPut]
        public async Task<MessageModel> update([FromBody] Model.ViewModel.update.updateNews updateNews)
        {
            MessageModel messageModel = new MessageModel();
            var newsData = await _newsService.GetOneById(updateNews.news.Id);
            if (newsData != null)
            {
                newsData.Content = updateNews.news.Content;
                newsData.Title = updateNews.news.Title;
                newsData.ImagePath = updateNews.news.ImagePath;
                newsData.LastChangTime = DateTime.Now;
                newsData.UserId = updateNews.news.UserId;
                newsData.State = updateNews.news.State;
                newsData.IsRemove = updateNews.news.IsRemove;
                newsData.ImagePath = updateNews.news.ImagePath;
                if (await _newsService.Update(newsData))
                {
                    var oldNewsToCategoryList = await _newsToCategoryService.Query(m => m.NewsId == updateNews.news.Id);
                    for (int i = 0; i < oldNewsToCategoryList.Count; i++)
                    {
                        await _newsToCategoryService.Delete(oldNewsToCategoryList[i]);
                    }
                    for (int j = 0; j < updateNews.cateId.Count; j++)
                    {
                        await _newsToCategoryService.Create(new NewsToCategory()
                        {
                            NewsId = updateNews.news.Id,
                            CategoryId = updateNews.cateId[j],
                        });
                    }

                }

            }
            messageModel.Code = 200;
            return messageModel;
        }
        #endregion

        #region 获取当前用户所有文章列表
        [HttpGet]

        public async Task<MessageModel> GetAllByUserId(int userId, QueryModel queryModel, int state)
        {
            MessageModel messageModel = new MessageModel();
            var newsList = await _newsService.GetAllByUserId(userId, queryModel, state);
            messageModel.Data = newsList;
            messageModel.Code = 200;
            messageModel.Msg = "获取成功";
            return messageModel;

        }
        #endregion

        #region 增加浏览量

        private async Task<MessageModel> Browse(int newsId)
        {
            MessageModel messageModel = new MessageModel();
            try
            {
                var newsData = await _newsService.GetOneById(newsId);
                if (newsData == null) messageModel.Msg = "该文章不存在";
                else
                {
                    newsData.BrowseCOunt = newsData.BrowseCOunt + 1;
                    await _newsService.Update(newsData);
                    messageModel.Msg = "浏览成功";
                }
                messageModel.Code = 200;
            }
            catch (Exception)
            {

                throw;
            }
            return messageModel;
        }
        #endregion

        #region 热门评论文章
        [HttpGet]
        public async Task<MessageModel> HotCommentNews(QueryModel queryModel)
        {


            MessageModel messageModel = new MessageModel();
            try
            {
                messageModel.Data = await _newsService.HotCommentNews(queryModel);
                messageModel.Msg = "获取成功";
            }
            catch (Exception)
            {
                throw;

            }
            messageModel.Code = 200;

            return messageModel;
        }
        #endregion

    }
}
