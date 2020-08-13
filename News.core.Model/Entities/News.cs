using System;
using System.Collections.Generic;

namespace News.core.Model.Entities
{
    //文章表
    public partial class News : BaseEntity
    {



        public string Title { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }

        public int? UserId { get; set; }
        public int BrowseCOunt { get; set; }
        public DateTime LastChangTime { get; set; }

        public virtual Users User { get; set; }
      

    }
}
