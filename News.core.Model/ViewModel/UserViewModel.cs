using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Model.ViewModel
{
    public class UserViewModel
    {
       
        public string Email { get; set; }
        public string PassWord { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public int IsRemove { get; set; } = 0;
        public int State { get; set; } = 0;
        public string Code { get; set; }
      
    }
}
