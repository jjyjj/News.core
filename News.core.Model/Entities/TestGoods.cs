using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Model.Entities
{
    public class TestGoods : BaseEntity
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public double Price { get; set;  }
    }
}
