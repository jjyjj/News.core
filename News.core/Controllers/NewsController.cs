using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News.core.IServices;
using News.core.Model;
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

        public NewsController(INewsService newsService, IUserService userService)
        {
            _newsService = newsService ?? throw new ArgumentNullException(nameof(newsService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        #region 获取文章列表
        [HttpGet]
        public async Task<MessageModel> GetAll()
        {
            return new MessageModel()
            {
                Code = 200,
                Msg = "获取成功",
                Data = await _newsService.GetAll()
            };

        }
        #endregion

        #region 获取文章详情
        [HttpGet]
        public async Task<MessageModel> GetDetailsById(int newsId)
        {
            MessageModel messageModel = new MessageModel();
            try
            {
                messageModel.Data = await _newsService.GetDetailsById(newsId);
                messageModel.Code = 200;
                messageModel.Msg = "获取文章详情成功";
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
