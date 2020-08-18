using System;
using System.Collections.Generic;

namespace News.core.Model.Entities
{
    //评论表
    public partial class Comments : BaseEntity
    {
       
   

        public string Content { get; set; }

        public string UserName { get; set; }
        public int? NewsId { get; set; }
        public int? UserId { get; set; }

        public virtual News News { get; set; }
        public virtual Users User { get; set; }

    }
}
