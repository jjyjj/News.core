using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News.core.IServices;
using News.core.Model;
using News.core.Model.Entities;
using News.core.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<MessageModel> Add([FromBody] NewsViewModel newsViewModel)
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
                var newsId = await _newsService.Create(news);

                if (newsId < 0) messageModel.Msg = "创建失败";
                else
                {
                    try
                    {
                        foreach (var item in newsViewModel.categories)
                        {
                            NewsToCategory newsToCategory = new NewsToCategory();
                            newsToCategory.CategoryId = item.Id;
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
            }
            messageModel.Code = 200;
            return messageModel;
        }
        #endregion

        #region 获取文章列表
        [HttpGet]
        public async Task<MessageModel> GetAll(QueryModel queryModel)
        {
            MessageModel messageModel = new MessageModel();

            try
            {
                if (queryModel.isPage)
                {

                    messageModel.Data = await _newsService.GetAll(queryModel.pageIndex, queryModel.pageSize);
                }
                else
                {

                    messageModel.Data = await _newsService.GetAll();

                }
                messageModel.Code = 200;
                messageModel.Msg = "获取成功";
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
                    messageModel.Data = await _newsService.GetDetailsById(newsId);
                    messageModel.Msg = "获取文章详情成功";
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

        #region 根据类别获取文章
        [HttpGet]
        public async Task<MessageModel> GetAllByCategory(string categoryNam)
        {
            MessageModel messageModel = new MessageModel();
            try
            {
                var categoryDate = (await _categoryService.GetAll()).Find(m => m.Name == categoryNam);
                if (categoryDate == null) messageModel.Msg = "该类别不存在";
                else
                {
                    messageModel.Data = await _newsService.GetAllByCategory(categoryNam);
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
            var newsData = await _newsService.GetOneById(newsId);
            if (newsData == null) messageModel.Msg = "文章不存在";
            else
            {
                //先去删除关联表
                var newsTocategoryList = (await _newsToCategoryService.GetAll()).FindAll(s => s.NewsId == newsId);
                foreach (var item in newsTocategoryList)
                {
                    await _newsToCategoryService.Delete(item);
                }

                //在删除文章表
                await _newsService.Delete(newsData);
                messageModel.Msg = "删除成功";
            }
            messageModel.Code = 200;
            return messageModel;

        }
        #endregion


        #region 浏览量
        [HttpPut]
        public async Task<MessageModel> Browse(int newsId)
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


    }
}
