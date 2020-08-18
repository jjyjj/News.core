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
        public async Task<MessageModel> Add(string categoryName, int userId)
        {
            MessageModel messageModel = new MessageModel();
            try
            {

                var categoryData = (await _categoryService.GetAll()).Find(m => m.Name == categoryName);

                if (categoryData == null)
                {
                    Category category = new Category();
                    category.Name = categoryName;
                    category.UserId = userId;
                    var result = await _categoryService.Create(category) > 0;
                    if (result) messageModel.Msg = "创建类别成功";
                    else messageModel.Msg = "创建类别失败";




                }
                else messageModel.Msg = "已存在该类别";

                messageModel.Code = 200;
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
                var categoryDate = await _categoryService.GetOneById(CategoryId);
                if (categoryDate == null) messageModel.Msg = "该类别不存在";
                else
                {
                    var newsToCategoryData = await _newsToCategoryService.GetOneById(CategoryId);
                    if (newsToCategoryData != null) messageModel.Msg = "无法删除，请检查该类别下是否还有其他新闻";
                    else
                    {
                        var isDel = await _categoryService.Delete(categoryDate);
                        if (isDel) messageModel.Msg = "删除成功";
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

        #region 修改类别
        [HttpPut]
        public async Task<MessageModel> Update(int categoryId, string categoryName)
        {
            MessageModel messageModel = new MessageModel();
            try
            {
                var data = await _categoryService.GetOneById(categoryId);

                if (data == null) messageModel.Msg = "类别不存在";
                else
                {
                    data.Name = categoryName;
                    var result = await _categoryService.Update(data);
                    if (result) messageModel.Msg = "修改类别成功";
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
