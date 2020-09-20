using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Model.ViewModel
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public Model.Entities.News News { get; set; }
        public List<Model.Entities.Comments> Comments { get; set; } = new List<Comments>();
        public CommentsChild[] commentsChildren { get; set; }
    }

}
