using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Model.Entities
{
    public class Imgs : BaseEntity
    {
        public string Url { get; set; }
        public int UserId { get; set; }
        public virtual Users User { get; set; }
    }
}
