using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Model.ViewModel
{
   public class AddArticleModel
    {
        
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public int UserId { get; set; }
     
      
        public List<int> categories { get; set; }
    }
}
