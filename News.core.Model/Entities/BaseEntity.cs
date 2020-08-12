using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Model.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime? CreateTime { get; set; } = DateTime.Now;
        public int? State { get; set; } = 0;
        public int? IsRemove { get; set; } = 0;
    }
}
