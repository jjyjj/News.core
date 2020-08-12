using System;
using System.Collections.Generic;

namespace News.core.Model.Entities
{
    public partial class News: BaseEntity
    {
        public News()
        {
            Comments = new HashSet<Comments>();
        }

    
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
   
        public int? UserId { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
    }
}
