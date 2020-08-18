using Microsoft.AspNetCore.Mvc;
using News.core.IServices;
using News.core.Model;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.core.Controllers
{
    public class CommentController : BaseController
    {
        private readonly ICommentService _commentService;
        private readonly INewsService _newsService;
        private readonly IUserService _userService;
        private readonly ICommentChildService _commentChildService;

        public CommentController(ICommentService commentService, INewsService newsService, IUserService userService, ICommentChildService commentChildService)
        {
            _commentService = commentService ?? throw new ArgumentNullException(nameof(commentService));
            _newsService = newsService ?? throw new ArgumentNullException(nameof(newsService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _commentChildService = commentChildService ?? throw new ArgumentNullException(nameof(commentChildService));
        }
        #region 添加评论


        [HttpPost]
        public async Task<MessageModel> Add(int newsId, int userId, string userName, string content)
        {
            MessageModel messageModel = new MessageModel();
            Model.Entities.Comments comments = new Model.Entities.Comments();

            comments.Content = content;
            comments.NewsId = newsId;
            comments.UserId = userId;
            comments.UserName = userName;
            try
            {
                var isCreate = await _commentService.Create(comments) > 0;
                if (isCreate)
                {
                    messageModel.Code = 200;
                    messageModel.Msg = "添加评论成功";
                }
            }
            catch (Exception)
            {

                throw;
            }
            return messageModel;



        }
        #endregion

        #region 查询评论
        [HttpGet]
        public async Task<MessageModel> GetAll(int newsId)
        {
            MessageModel messageModel = new MessageModel();
            try
            {

                messageModel.Data = await _commentService.GetAll(newsId);
                messageModel.Code = 200;
                messageModel.Msg = "success";
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
