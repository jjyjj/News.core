using System;
using System.Collections.Generic;

namespace News.core.Model.Entities
{
    public partial class Users : BaseEntity
    {
        public Users()
        {
            Comments = new HashSet<Comments>();
            News = new HashSet<News>();
        }


        public string Email { get; set; }
        public string PassWord { get; set; }


        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<News> News { get; set; }
    }
}
