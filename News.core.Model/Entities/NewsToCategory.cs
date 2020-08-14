using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Model.Entities
{
    /// <summary>
    /// 文章类别关联表
    /// </summary>
    public partial class NewsToCategory : BaseEntity
    {
        public int? NewsId { get; set; }
        public virtual News News { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

    }
}
