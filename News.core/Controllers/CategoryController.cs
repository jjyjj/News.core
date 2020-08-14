using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News.core.IServices;
using News.core.Model;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.core.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;


        private readonly INewsToCategoryService _newsToCategoryService;

        public CategoryController(ICategoryService categoryService, IUserService userService, INewsService newsService, INewsToCategoryService newsToCategoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));

            _newsToCategoryService = newsToCategoryService ?? throw new ArgumentNullException(nameof(newsToCategoryService));
        }
        #region 获取所有类别
        [HttpGet]
        public async Task<MessageModel> getAll()
        {
            MessageModel messageModel = new MessageModel();
            try
            {
                var data = await _categoryService.GetAll();
                messageModel.Code = 200;
                messageModel.Data = data;
                messageModel.Msg = "获取成功";

            }
            catch (Exception)
            {

                throw;
            }
            return messageModel;
        }
        #endregion

        #region 增加 类别
        [HttpPost]
        public async Task<MessageModel> Add(Category category, int userId)
        {
            MessageModel messageModel = new MessageModel();
            try
            {
                var data = await _categoryService.GetAll();


                if (data.Exists(m => m.Name == category.Name))
                {
                    category.User.Id = userId;
                    var result = await _categoryService.Create(category) > 0;
                    if (result)
                    {
                        messageModel.Code = 200;
                        messageModel.Msg = "创建类别成功";
                    }
                }
                messageModel.Code = 200;
                messageModel.Msg = "已存在该类别";
            }
            catch (Exception)
            {

                throw;
            }
            return messageModel;
        }
        #endregion

        #region 删除类别
        [HttpDelete]
        public async Task<MessageModel> Del(int CategoryId)
        {
            MessageModel messageModel = new MessageModel();
            try
            {
                //获取到的同样是本表的数据 没有外键数据，说明需要进行关联查询
                var data = await _newsToCategoryService.GetAll();
                data = data.FindAll(m => m.Category.Id == CategoryId);

                if (data.Count <= 0)//没有新闻关联,进行删除
                {
                    var result = await _categoryService.Delete(new Category() { Id = CategoryId });
                    if (result)
                    {
                        messageModel.Code = 200;
                        messageModel.Msg = "删除成功";
                    }
                }
                else
                {
                    messageModel.Msg = "无法删除，请检查该类别下是否还有其他新闻";
                }

            }
            catch (Exception)
            {

                throw;
            }
            return messageModel;
        }
        #endregion

        #region 修改类别
        [HttpPut]
        public async Task<MessageModel> Update(int categoryId, string categoryName)
        {
            MessageModel messageModel = new MessageModel();
            try
            {
                var data = await _categoryService.GetOneById(categoryId);

                if (data != null)
                {
                    data.Name = categoryName;
                    var result = await _categoryService.Update(data);
                    if (result)
                    {
                        messageModel.Code = 200;
                        messageModel.Msg = "修改类别成功";
                    }
                }
                else
                {
                    messageModel.Msg = "类别不存在";
                }

            }
            catch (Exception)
            {


            }
            return messageModel;
        }
        #endregion
    }
}
