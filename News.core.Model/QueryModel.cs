using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Model
{
    //查询通用类
    public class QueryModel
    {
        /// <summary>
        /// 是否分页
        /// </summary>
        public bool isPage { get; set; } = false;

        /// <summary>
        /// 当前页面
        /// </summary>
        public int pageIndex { get; set; }
        /// <summary>
        /// 页面大小
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 查询字段
        /// </summary>
        public string strWhere { get; set; }


    }
}
