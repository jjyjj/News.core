using Microsoft.AspNetCore.Mvc;
using News.core.IServices;
using News.core.Model;
using News.core.Model.Entities;
using Newtonsoft.Json.Linq;
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
        public async Task<MessageModel> Add([FromBody]  Comments comments)
        {
            MessageModel messageModel = new MessageModel();


            if (comments.UserId > 0 && comments.NewsId > 0)
            {
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
            }
            return messageModel;



        }
        #endregion

        #region 查询评论
        [HttpGet]
        public async Task<MessageModel> GetAll(int newsId, QueryModel queryModel)
        {
            MessageModel messageModel = new MessageModel();
            try
            {

                messageModel.Data = await _commentService.GetAll(newsId, queryModel);
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

        #region 删除评论
        [HttpDelete]
        public async Task<MessageModel> Del(int commentId)
        {
            MessageModel messageModel = new MessageModel();
            if (commentId > 0)
            {
                if (await _commentService.DelAllComment(commentId))
                {
                    messageModel.Msg = "删除成功";
                }
                messageModel.Msg = "删除失败";
            }
            messageModel.Msg = "评论不存在";
            messageModel.Code = 200;
            return messageModel;
        }
        #endregion

        [HttpGet]
        public async Task<MessageModel> GetAllByUserId(int userId, QueryModel queryModel)
        {
            MessageModel messageModel = new MessageModel();
            try
            {

                messageModel.Data = await _commentService.GetAllByUserId(userId, queryModel);
                messageModel.Code = 200;
                messageModel.Msg = "success";
            }

            catch (Exception)
            {

                throw;
            }
            return messageModel;
        }
    }
}
