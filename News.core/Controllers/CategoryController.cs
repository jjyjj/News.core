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
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }
        #region 获取所有类别
        [HttpGet]
        public async Task<MessageModel> getAll()
        {
            return new MessageModel()
            {
                Code = 200,
                Data = await _categoryService.GetAll(),
                Msg = "请求成功"
            };
        }
        #endregion

        #region 增加 类别
        [HttpPost]
        public async Task<MessageModel> Add(string categoryName, int userId)
        {
            var id = await _categoryService.Add(categoryName, userId);
            return new MessageModel()
            {
                Code = 200,
                Data = id,
                Msg = id > 0 ? "创建成功" : "创建失败"
            };
        }
        #endregion

        #region 删除类别
        [HttpDelete]
        public async Task<MessageModel> Del(int CategoryId)
        {
            var isDel = await _categoryService.Del(CategoryId);
            return new MessageModel()
            {
                Code = 200,
                Data = isDel,
                Msg = isDel ? "删除成功" : "删除失败"
            };
        }
        #endregion

        #region 修改类别
        [HttpPut]
        public async Task<MessageModel> Update(int categoryId, string categoryName)
        {
            var isUpdata = await _categoryService.Update(categoryId, categoryName);
            return new MessageModel()
            {
                Code = 200,
                Data = isUpdata,
                Msg = isUpdata ? "更新成功" : "更新失败"
            };
        }
        #endregion
    }
}
