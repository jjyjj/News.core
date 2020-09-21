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
   

        public NewsController(INewsService newsService)
        {
            _newsService = newsService ?? throw new ArgumentNullException(nameof(newsService));
           
        }
        #region 添加文章
        [HttpPost]
        public async Task<MessageModel> Add([FromBody] AddArticleModel newsViewModel, int state)
        {
            #region old
            //MessageModel messageModel = new MessageModel();
            //var user = await _userService.GetOneById(newsViewModel.UserId);
            //if (user == null) messageModel.Msg = "用户不存在";
            //else
            //{
            //    Model.Entities.News news = new Model.Entities.News();

            //    news.Title = newsViewModel.Title;
            //    news.UserId = newsViewModel.UserId;
            //    news.Content = newsViewModel.Content;
            //    news.State = state;
            //    news.ImagePath = newsViewModel.ImagePath;


            //    var newsId = await _newsService.Create(news);

            //    if (newsId < 0) messageModel.Msg = "创建失败";
            //    else
            //    {
            //        try
            //        {
            //            foreach (var item in newsViewModel.categories)
            //            {
            //                NewsToCategory newsToCategory = new NewsToCategory();
            //                newsToCategory.CategoryId = item;
            //                newsToCategory.NewsId = newsId;
            //                await _newsToCategoryService.Create(newsToCategory);
            //            }
            //            messageModel.Msg = "创建新闻成功";
            //        }
            //        catch (Exception)
            //        {
            //            var data = await _newsService.GetOneById(newsId);
            //            await _newsService.Delete(data);
            //            messageModel.Msg = "创建失败";
            //        }
            //    }
            //    messageModel.Data = newsId;
            //}
            //messageModel.Code = 200;

            //return messageModel;
            #endregion

            #region new
            var id = await _newsService.Add(newsViewModel, state);
            return new MessageModel()
            {
                Code = 200,
                Data = id,
                Msg = id > 0 ? "创建成功" : "创建失败"
            };
            #endregion
        }
        #endregion

        #region 获取文章列表
        [HttpGet]
        public async Task<MessageModel> GetAll(QueryModel queryModel, int? state)
        {
            #region old
            //MessageModel messageModel = new MessageModel();

            //try
            //{
            //    if (!queryModel.isPage) messageModel.Data = await _newsService.Query(m => m.State == 0);
            //    else
            //    {
            //        var newsList = await _newsService.Pagination<Model.Entities.News, object>(queryModel.pageIndex, queryModel.pageSize, n => n.CreateTime, x => x.State == state, queryModel.isDesc);
            //        foreach (var item in newsList.data)
            //        {
            //            item.User = await _userService.GetOneById(item.UserId.Value);
            //        }
            //        messageModel.Data = newsList;
            //    }
            //    messageModel.Code = 200;
            //    messageModel.Msg = "获取数据成功";
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            //return messageModel;
            #endregion

            #region new
            var list = await _newsService.GetAll(queryModel, state);
            return new MessageModel()
            {
                Code = 200,
                Data = list,
                Msg = list == null ? "获取失败" : "获取成功"
            };
            #endregion

        }
        #endregion

        #region 获取文章详情
        [HttpGet]
        public async Task<MessageModel> GetDetailsById(int newsId)
        {
            #region old
            //MessageModel messageModel = new MessageModel();

            //var newsData = await _newsService.GetOneById(newsId);
            //if (newsData == null) messageModel.Msg = "不存在该文章";
            //else
            //{
            //    messageModel.Data = await _newsService.GetDetailsById(newsId);
            //    await Browse(newsId);
            //    messageModel.Msg = "获取文章详情成功";
            //}
            //messageModel.Code = 200;

            #endregion
            var list = await _newsService.GetDetailsById(newsId);
            return new MessageModel()
            {
                Code = 200,
                Data = list,
                Msg = list == null ? "获取失败" : "获取成功"
            };

        }
        #endregion

        #region 根据类别获取文章列表
        [HttpGet]
        public async Task<MessageModel> GetAllByCategory(string categoryName, QueryModel queryModel)
        {
            #region old
            //MessageModel messageModel = new MessageModel();

            //var categoryDate = await _categoryService.GetOneByStr(m => m.Name == categoryName);
            //if (categoryDate == null) messageModel.Msg = "该类别不存在";
            //else
            //{
            //    messageModel.Data = await _newsService.GetAllByCategory(categoryName, queryModel);
            //    messageModel.Msg = "获取成功";
            //}
            //messageModel.Code = 200;
            #endregion

            var list = await _newsService.GetAllByCategory(categoryName, queryModel);
            return new MessageModel()
            {
                Code = 200,
                Data = list,
                Msg = list == null ? "获取失败" : "获取成功"
            };
        }
        #endregion

        #region 删除文章
        [HttpDelete]
        public async Task<MessageModel> Del(int newsId)
        {

            var isDel = await _newsService.Del(newsId);
            return new MessageModel()
            {
                Code = 200,
                Data = isDel,
                Msg = isDel ? "删除成功" : "删除失败"
            };

        }
        #endregion


        #region 修改文章
        [HttpPut]
        public async Task<MessageModel> update([FromBody] Model.ViewModel.update.updateNews updateNews)
        {
            #region old
            //MessageModel messageModel = new MessageModel();
            //var newsData = await _newsService.GetOneById(updateNews.news.Id);
            //if (newsData != null)
            //{
            //    newsData.Content = updateNews.news.Content;
            //    newsData.Title = updateNews.news.Title;
            //    newsData.ImagePath = updateNews.news.ImagePath;
            //    newsData.LastChangTime = DateTime.Now;
            //    newsData.UserId = updateNews.news.UserId;
            //    newsData.State = updateNews.news.State;
            //    newsData.IsRemove = updateNews.news.IsRemove;
            //    newsData.ImagePath = updateNews.news.ImagePath;
            //    if (await _newsService.Update(newsData))
            //    {
            //        var oldNewsToCategoryList = await _newsToCategoryService.Query(m => m.NewsId == updateNews.news.Id);
            //        for (int i = 0; i < oldNewsToCategoryList.Count; i++)
            //        {
            //            await _newsToCategoryService.Delete(oldNewsToCategoryList[i]);
            //        }
            //        for (int j = 0; j < updateNews.cateId.Count; j++)
            //        {
            //            await _newsToCategoryService.Create(new NewsToCategory()
            //            {
            //                NewsId = updateNews.news.Id,
            //                CategoryId = updateNews.cateId[j],
            //            });
            //        }

            //    }

            //}
            //messageModel.Code = 200;
            //return messageModel;
            #endregion
            var isUpdate = await _newsService.Update(updateNews);
            return new MessageModel()
            {
                Code = 200,
                Data = isUpdate,
                Msg = isUpdate ? "更新成功" : "更新失败"
            };
        }
        #endregion

        #region 获取当前用户所有文章列表
        [HttpGet]

        public async Task<MessageModel> GetAllByUserId(int userId, QueryModel queryModel, int state)
        {

            var list = await _newsService.GetAllByUserId(userId, queryModel, state);
            return new MessageModel()
            {
                Code = 200,
                Data = list,
                Msg = list == null ? "获取失败" : "获取成功"
            };

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
            var list = await _newsService.HotCommentNews(queryModel);
            return new MessageModel()
            {
                Code = 200,
                Data = list,
                Msg = list == null ? "获取失败" : "获取成功"
            };



        }
        #endregion

    }
}
