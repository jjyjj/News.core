using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Model.Entities
{
    //评论子表
    public partial class CommentsChild : BaseEntity
    {
        public int? UserId { get; set; }
        public int? NewsId { get; set; }
        public int? CommentsId { get; set; }
        public string Content { get; set; }

        public virtual News News { get; set; }
        public virtual Users User { get; set; }
        public virtual Comments Comments { get; set; }
    }
}
