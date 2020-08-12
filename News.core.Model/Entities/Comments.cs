using System;
using System.Collections.Generic;

namespace News.core.Model.Entities
{
    public partial class Comments: BaseEntity
    {
 
        public string Content { get; set; }
      
        public int? UserId { get; set; }
        public int? NewsId { get; set; }

        public virtual News News { get; set; }
        public virtual Users User { get; set; }
    }
}
