using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Model.ViewModel
{
    public class NewsViewModel : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public int UserId { get; set; }
        public int BrowseCount { get; set; } = 0;
        public DateTime LastChangTime { get; set; }
        public List<Category> categories { get; set; }

    }
}
