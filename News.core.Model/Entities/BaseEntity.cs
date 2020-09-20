using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace News.core.Model.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime? CreateTime { get; set; } = DateTime.Now;
        //public string CreateTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
        /// <summary>
        /// 状态
        /// News: 0正常 1草稿箱 2垃圾箱
        /// Users:0正常 1 冻结
        /// Comments：0正常 1屏蔽
        /// Imgs: 1轮播图片
        /// </summary>
        public int? State { get; set; } = 0;
        /// <summary>
        /// 是否删除
        /// </summary>
        public int? IsRemove { get; set; } = 0;

    }
}
