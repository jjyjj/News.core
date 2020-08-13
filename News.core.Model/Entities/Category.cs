using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Model.Entities
{
    //类别表
    public partial class Category : BaseEntity
    {
        public string Name { get; set; }//类别名字
        public virtual Users User { get; set; }

    }
}
