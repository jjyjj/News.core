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

        public CommentChildController(ICommentChildService commentChildService)
        {
            _commentChildService = commentChildService ?? throw new ArgumentNullException(nameof(commentChildService));

        }
        #region 添加子评论
        [HttpPost]
        public async Task<MessageModel> add([FromBody] CommentsChild commentsChild)
        {
            var id = await _commentChildService.Add(commentsChild);
            return new MessageModel()
            {
                Code = 200,
                Data = id,
                Msg = id > 0 ? "创建成功" : "创建失败"
            };
        }
        #endregion

        #region 删除子评论
        [HttpDelete]
        public async Task<MessageModel> Del(int commentChildId)
        {
            var isDel = await _commentChildService.Del(commentChildId);
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
