using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Model.Entities
{
    //评论子表
    public partial class CommentsChild : BaseEntity
    {
        /// <summary>
        /// 评论人id
        /// </summary>
        public int? UserId { get; set; }
        /// <summary>
        /// 文章id
        /// </summary>
        public int? NewsId { get; set; }
        /// <summary>
        /// 父评论id
        /// </summary>
        public int? CommentsId { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 父评论的用户名
        /// </summary>
        public string UserName { get; set; }
        public virtual News News { get; set; }
        public virtual Users User { get; set; }
        public virtual Comments Comments { get; set; }
    }
}
