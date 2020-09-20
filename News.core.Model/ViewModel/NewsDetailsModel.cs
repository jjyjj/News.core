using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Model.ViewModel
{
    public class NewsDetailsModel
    {
        //public int Id { get; set; }//文章Id

        //public string Title { get; set; }//标题
        //public string Content { get; set; }//内容


        //public int BrowseCOunt { get; set; }//浏览量
        //public DateTime CreateTime { get; set; }//创建时间
        //public DateTime LastChangTime { get; set; }//最后修改时间
        //public int UserId { get; set; }//创建人Id
        //public string UserName { get; set; }//创建人Name
        public Model.Entities.News News { get; set; }
    
        public List<Model.Entities.Category> Categories { get; set; } = new List<Category>();//类别
        public string previous { get; set; }//上一篇


        public int previousId { get; set; }//上一篇Id

        public string next { get; set; }//下一篇


        public int nextId { get; set; }//下一篇Id





    }
}
