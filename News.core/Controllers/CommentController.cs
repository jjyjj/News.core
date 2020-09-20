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

        public CommentController(ICommentService commentService, INewsService newsService, IUserService userService, ICommentChildService commentChildService)
        {
            _commentService = commentService ?? throw new ArgumentNullException(nameof(commentService));
            
        }
        #region 添加评论
        [HttpPost]
        public async Task<MessageModel> Add([FromBody]  Comments comments)
        {
            var id = await _commentService.Add(comments);
            return new MessageModel()
            {
                Code = 200,
                Data = id,
                Msg = id > 0 ? "创建成功" : "创建失败"
            };



        }
        #endregion

        #region 查询所有评论
        [HttpGet]
        public async Task<MessageModel> GetAll(int newsId, QueryModel queryModel)
        {
            var list = await _commentService.GetAll(newsId, queryModel);

            return new MessageModel()
            {
                Code = 200,
                Data = list,
                Msg = list == null ? "暂无数据" : "获取成功"
            };
        }


        #endregion
      
        #region 根据用户查询评论
        [HttpGet]
        public async Task<MessageModel> GetAllByUserId(int userId, QueryModel queryModel)
        {
            var list = await _commentService.GetAllByUserId(userId, queryModel);
            return new MessageModel()
            {
                Code = 200,
                Data = list,
                Msg = list == null ? "暂无数据" : "获取成功"
            };
        }
        #endregion
     
        #region 删除评论
        [HttpDelete]
        public async Task<MessageModel> Del(int commentId)
        {
            var isDel = await _commentService.Del(commentId);
            return new MessageModel()
            {
                Code = 200,
                Data = isDel,
                Msg = isDel ? "删除成功" : "删除失败"
            };
        }
        #endregion


    }
}
