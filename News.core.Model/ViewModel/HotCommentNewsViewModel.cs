using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Model.ViewModel
{
    public class HotCommentNewsViewModel
    {
        public int Id { get; set; }

        public Model.Entities.News News { get; set; }
        public int CommentCount { get; set; }
    }
}
