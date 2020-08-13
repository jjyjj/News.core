using System;
using System.Collections.Generic;

namespace News.core.Model.Entities
{
    //用户表
    public partial class Users : BaseEntity
    {


        public string Email { get; set; }
        public string PassWord { get; set; }

        public string Phone { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public string Birthday { get; set; }//2020-02-02
        public string Sex { get; set; }//0男  1女
        public string ImgUrl { get; set; }
        //级别
        public string Level { get; set; } = "0";//0 普通用户 1 管理员 2 特殊管理员


    }
}
