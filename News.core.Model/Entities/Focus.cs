using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Model.Entities
{
    //粉丝表
    public partial class Focus : BaseEntity
    {
       
        public int UserId { get; set; }
        public int FocusId { get; set; }
        public virtual Users User { get; set; }
    }
}
