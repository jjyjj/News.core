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
    public class CommentChildController : BaseController
    {
        private readonly ICommentChildService _commentChildService;
        private readonly ICommentService _commentService;
        private readonly INewsService _newsService;
        private readonly IUserService _userService;

        public CommentChildController(ICommentChildService commentChildService, ICommentService commentService, INewsService newsService, IUserService userService)
        {
            _commentChildService = commentChildService ?? throw new ArgumentNullException(nameof(commentChildService));
            _commentService = commentService ?? throw new ArgumentNullException(nameof(commentService));
            _newsService = newsService ?? throw new ArgumentNullException(nameof(newsService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
        #region 添加子评论
        [HttpPost]
        public async Task<MessageModel> add([FromBody] CommentsChild commentsChild)
        {
            MessageModel messageModel = new MessageModel();

            if (commentsChild.UserId == null || commentsChild.NewsId == null || commentsChild.Content == null) messageModel.Msg = "添加评论失败";
            else
            {
                var isCreat = await _commentChildService.Create(commentsChild) > 0;
                if (isCreat)
                {
                    messageModel.Msg = "评论成功";
                }
            }
            messageModel.Code = 200;
            return messageModel;
        }
        #endregion

        #region 删除子评论
        [HttpDelete]
        public async Task<MessageModel> Del(int commentChildId)
        {
            MessageModel messageModel = new MessageModel();
            if (commentChildId > 0)
            {
                var isDel = await _commentChildService.Delete(new CommentsChild() { Id = commentChildId });
                if (isDel) messageModel.Msg = "删除成功";
                else messageModel.Msg = "删除失败";

                messageModel.Code = 200;
            }
            return messageModel;
        }
        #endregion


    }
}
